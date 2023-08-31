using Images;
using Pokemon;
namespace Telegram;
public class PollingService
{
    private readonly IMessageService _messageService;
    private readonly IPokeFuseManager _pokeFuseManager;
    private readonly IImageManager _imageManager;
    private static int _lastUpdateId = 0;

    public PollingService(IMessageService messageService, IPokeFuseManager pokeFuseManager, IImageManager imageManager)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _pokeFuseManager = pokeFuseManager ?? throw new ArgumentNullException(nameof(pokeFuseManager));
        _imageManager = imageManager ?? throw new ArgumentNullException(nameof(imageManager));
    }

    public async Task PollForUpdatesAsync()
    {
        try
        {
            var updates = await _messageService.GetUpdates(_lastUpdateId);

            if (updates?.result == null) return;
            foreach (var result in updates.result)
            {
                await ProcessUpdate(result);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}. {e.StackTrace}");
        }
    }

    private async Task ProcessUpdate(Result update)
    {
        var text = update.message?.text ?? update.edited_message?.text ?? "";
        Console.WriteLine($"Received message: {text}");
        _lastUpdateId = update.update_id;

        var pokeFuseResponse = _pokeFuseManager.GetFuseFromMessage(text);
        if (pokeFuseResponse != null)
        {
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
                await SendStickerIfAvailable(update.message.chat.id, pokeFuseResponse.GetWebpUrl(), pokeFuseResponse.GetCaption1());
            }
            if (pokeFuseResponse.ImageUrl1 != pokeFuseResponse.ImageUrl2 && image2Valid)
            {
                await _imageManager.ConvertPngUrlToWebpAsync(pokeFuseResponse.ImageUrl2, pokeFuseResponse.GetWebpUrl());
                await SendStickerIfAvailable(update.message.chat.id, pokeFuseResponse.GetWebpUrl(), pokeFuseResponse.GetCaption2());
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