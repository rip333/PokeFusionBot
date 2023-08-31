using Pokemon;
namespace Telegram;
public class PollingService
{
    private readonly IMessageService _messageService;
    private readonly IPokeFuseManager _pokeFuseManager;
    private static int _lastUpdateId = 0;

    public PollingService(IMessageService messageService, IPokeFuseManager pokeFuseManager)
    {
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _pokeFuseManager = pokeFuseManager ?? throw new ArgumentNullException(nameof(pokeFuseManager));
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
            await SendImageIfAvailable(update.message.chat.id, pokeFuseResponse.ImageUrl1, pokeFuseResponse.GetCaption1());
            if (pokeFuseResponse.ImageUrl1 != pokeFuseResponse.ImageUrl2)
            {
                await SendImageIfAvailable(update.message.chat.id, pokeFuseResponse.ImageUrl2, pokeFuseResponse.GetCaption2());
            }
        }
    }

    private async Task SendImageIfAvailable(long chatId, string imageUrl, string caption)
    {
        if (!await _messageService.CheckFor404(imageUrl))
        {
            await _messageService.SendImageToChat(chatId, imageUrl, caption);
        }
    }
}