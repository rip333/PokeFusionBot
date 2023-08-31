using Pokemon;
using Telegram;

Console.WriteLine("PokeFusionBot, I choose you!");

var token = await ConfigUtility.GetToken();
var pollingService = new PollingService(new MessageService(token, new HttpClient()), new PokeFuseManager());

while (true)
{
    await pollingService.PollForUpdatesAsync();
    await Task.Delay(2000);
}

