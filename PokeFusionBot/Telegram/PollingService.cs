using Images;
using PokeFusionBot.ChatGptFunctions;
using PokemonFunctions;
namespace Telegram;
public class PollingService
{
    private readonly IMessageService _messageService;
    private readonly IPokeFuseManager _pokeFuseManager;
    private readonly IImageManager _imageManager;
    private readonly IPokeApi _pokeApi;
    public readonly IBattleManager _battleManager;
    private static int _lastUpdateId = 0;

    public PollingService(IMessageService messageService, IPokeFuseManager pokeFuseManager, IImageManager imageManager,
    IPokeApi pokeApi, IBattleManager battleManager)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _pokeFuseManager = pokeFuseManager ?? throw new ArgumentNullException(nameof(pokeFuseManager));
        _imageManager = imageManager ?? throw new ArgumentNullException(nameof(imageManager));
        _pokeApi = pokeApi ?? throw new ArgumentNullException(nameof(pokeApi));
        _battleManager = battleManager;
    }

    public async Task PollForUpdatesAsync()
    {
        try
        {
            var updates = await _messageService.GetUpdates(_lastUpdateId);

            if (updates?.result == null) return;
            foreach (var result in updates.result)
            {
                _lastUpdateId = result.update_id;
                await ProcessUpdate(result);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}. {e.StackTrace}");
        }
    }

    private async Task ProcessUpdate(Result result)
    {
        var text = result.message?.text ?? "";
        Console.WriteLine($"Received message: {text}");
        if (string.IsNullOrEmpty(text)) return;

        if (BattleManager.CheckStringFormat(text))
        {
            await _messageService.SendChatAction(result.message.chat.id, "typing");
            var battleText = await _battleManager.HandleBattle(text);
            if (!string.IsNullOrEmpty(battleText))
            {
                Console.WriteLine("Sending Battle Text...");
                await _messageService.SendMessageToChat(result.message.chat.id, battleText, "Markdown");
                return;
            }
        }


        var pokeFuseResponse = _pokeFuseManager.GetFuseFromMessage(text);
        if (pokeFuseResponse != null)
        {
            //pokeFuseResponse = await pokeFuseResponse.PopulatePokemonData(_pokeApi);
            bool image1Valid;
            bool image2Valid;

            if (pokeFuseResponse.IsRandom)
            {
                pokeFuseResponse = await pokeFuseResponse.GenerateRandomUntilValid(_imageManager);
                image1Valid = !await _imageManager.CheckFor404(pokeFuseResponse.ImageUrl1);
                image2Valid = !await _imageManager.CheckFor404(pokeFuseResponse.ImageUrl2);
            }
            else
            {
                image1Valid = !await _imageManager.CheckFor404(pokeFuseResponse.ImageUrl1);
                image2Valid = !await _imageManager.CheckFor404(pokeFuseResponse.ImageUrl2);
            }

            if (image1Valid)
            {
                await _imageManager.ConvertPngUrlToWebpAsync(pokeFuseResponse.ImageUrl1, pokeFuseResponse.GetWebpUrl());
                await SendStickerIfAvailable(result.message.chat.id, pokeFuseResponse.GetWebpUrl(), pokeFuseResponse.GetCaption1());
            }
            if (pokeFuseResponse.ImageUrl1 != pokeFuseResponse.ImageUrl2 && image2Valid)
            {
                await _imageManager.ConvertPngUrlToWebpAsync(pokeFuseResponse.ImageUrl2, pokeFuseResponse.GetWebpUrl());
                await SendStickerIfAvailable(result.message.chat.id, pokeFuseResponse.GetWebpUrl(), pokeFuseResponse.GetCaption2());
            }
        }
    }

    private async Task SendImageIfAvailable(long chatId, string imageUrl, string caption)
    {
        if (!await _imageManager.CheckFor404(imageUrl))
        {
            await _messageService.SendImageToChat(chatId, imageUrl, caption);
        }
    }

    private async Task SendStickerIfAvailable(long chatId, string webpUrl, string caption)
    {
        await _messageService.SendStickerToChat(chatId, webpUrl, caption);
    }
}