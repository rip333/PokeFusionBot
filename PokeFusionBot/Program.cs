using Images;
using PokeApiNet;
using PokemonFunctions;
using Telegram;
using PokeFusionBot.ChatGptFunctions;

Console.WriteLine("PokeFusionBot, I choose you!");

var botToken = await ConfigUtility.GetBotConfig();

var httpClient = new HttpClient();
var pollingService = new PollingService(
        new MessageService(botToken.TelegramToken, httpClient),
        new PokeFuseManager(),
        new ImageManager(httpClient),
        new PokeApi(new PokeApiClient(httpClient)),
        new BattleManager(new ChatGptWrapper(botToken.ChatGptToken))
    );

while (true)
{
    await pollingService.PollForUpdatesAsync();
    await Task.Delay(250);
}

