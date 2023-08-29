using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("PokeFusionBot, I choose you!");

var updateManager = new UpdateManager();
var token = await ConfigUtility.GetToken();
TelegramBotClient _bot;
_bot = new TelegramBotClient(token);
var me = await _bot.GetMeAsync();
Console.Title = me.Username ?? "Telegram Bot";
using var cts = new CancellationTokenSource();

while (true)
{
    try
    {
        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        _bot.StartReceiving(
            new DefaultUpdateHandler(updateManager.HandleUpdateAsync, updateManager.HandleErrorAsync),
            cts.Token);

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.Read();
        // Send cancellation request to stop bot
        cts.Cancel();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        Thread.Sleep(60000);
    }
}