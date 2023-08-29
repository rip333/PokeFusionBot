using System.Reflection;
using System.Text.Json;

namespace Telegram
{
    public class PollingService
    {
        private static HttpClient _httpClient = new HttpClient();
        private static int _lastUpdateId = 0;

        public static async Task PollForUpdatesAsync(string token)
        {
            var response = await _httpClient.GetStringAsync($"{Constants.API_URL}{token}/getUpdates?offset={_lastUpdateId + 1}");
            Console.WriteLine(response);

            try
            {
                var updates = JsonSerializer.Deserialize<TelegramResponse>(response);
                if (updates == null || updates.Result == null) return;
                foreach (var update in updates.Result)
                {
                    if (update.Message == null || update.Message.Text == null) break;
                    Console.WriteLine($"Received message: {update.Message.Text}");
                    _lastUpdateId = update.Update_id;

                    var url = await PokeFuseManager.GetFuseFromMessage(update.Message.Text);
                    if (string.IsNullOrEmpty(url)) return;
                    await MessageService.SendImageToChat(update.Message.Chat.Id, url, token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}. {e.StackTrace}");
            }
        }
    }
}