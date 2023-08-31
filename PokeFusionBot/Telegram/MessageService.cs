using System.Text.Json;

namespace Telegram
{
    public class MessageService : IMessageService
    {
        private HttpClient _httpClient;
        private string _token;

        public MessageService(string token, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _token = token;
        }

        public async Task<GetUpdateResponse> GetUpdates(int lastUpdateId)
        {
            var response = await _httpClient.GetStringAsync($"{Constants.API_URL}{_token}/getUpdates?offset={lastUpdateId + 1}&allowedUpdates=UpdateType.Message");
            var updates = JsonSerializer.Deserialize<GetUpdateResponse>(response);
            return updates;
        }

        public async Task SendImageToChat(long chatId, string imageUrl, string message)
        {
            var requestUrl = $"{Constants.API_URL}{_token}/sendPhoto";
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(chatId.ToString()), "chat_id" },
                { new StringContent(imageUrl), "photo" },
                { new StringContent(message), "caption"}
            };

            var response = await _httpClient.PostAsync(requestUrl, formContent);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString); // To see the server's response
        }

        public async Task SendChatAction(long chatId, string action = "upload_photo")
        {
            var requestUrl = $"{Constants.API_URL}{_token}/sendChatAction";
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(chatId.ToString()), "chat_id" },
                { new StringContent(action), "action"}
            };
            await _httpClient.PostAsync(requestUrl, formContent);
        }

        public async Task<bool> CheckFor404(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            return response.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
    }
}