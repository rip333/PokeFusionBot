namespace Telegram
{
    public class MessageService
    {
        private static HttpClient _httpClient = new HttpClient();

        public static async Task SendImageToChat(long chatId, string imageUrl, string token)
        {
            var requestUrl = $"{Constants.API_URL}{token}/sendPhoto";
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(chatId.ToString()), "chat_id" },
                { new StringContent(imageUrl), "photo" }
            };

            var response = await _httpClient.PostAsync(requestUrl, formContent);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString); // To see the server's response
        }
    }
}