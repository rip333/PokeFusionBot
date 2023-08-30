using System.Text.Json;

namespace Telegram
{
    public class PollingService
    {
        private static HttpClient _httpClient = new HttpClient();
        private static int _lastUpdateId = 0;

        public static async Task PollForUpdatesAsync(string token)
        {
            var response = await _httpClient.GetStringAsync($"{Constants.API_URL}{token}/getUpdates?offset={_lastUpdateId + 1}&allowedUpdates=UpdateType.Message");
            try
            {
                var updates = JsonSerializer.Deserialize<GetUpdateResponse>(response);

                if (updates != null && updates.result != null)
                    foreach (var update in updates.result)
                    {
                        var text = update.message?.text ?? update.edited_message?.text ?? "";
                        Console.WriteLine($"Received message: {text}");
                        _lastUpdateId = update.update_id;

                        var url = await PokeFuseManager.GetFuseFromMessage(text);
                        if (string.IsNullOrEmpty(url)) continue;
                        else
                        {
                            await MessageService.SendImageToChat(update.message.chat.id, url, token);
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}. {e.StackTrace}");
            }
        }
    }
}