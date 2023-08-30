using Telegram;

Console.WriteLine("PokeFusionBot, I choose you!");

var token = await ConfigUtility.GetToken();

while (true)
{
    await PollingService.PollForUpdatesAsync(token);
    await Task.Delay(2000);
}

