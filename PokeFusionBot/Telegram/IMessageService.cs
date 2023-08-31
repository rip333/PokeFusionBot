namespace Telegram;

public interface IMessageService
{
    Task<GetUpdateResponse> GetUpdates(int lastUpdateId);
    Task SendImageToChat(long chatId, string imageUrl, string message);
    Task SendStickerToChat(long chatId, string webpUrl, string message);
    Task SendChatAction(long chatId, string action = "upload_photo");
}