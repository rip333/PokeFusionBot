using Images;
using PokeApiNet;
using PokemonFunctions;
using Telegram;

Console.WriteLine("PokeFusionBot, I choose you!");

var token = await ConfigUtility.GetToken();

var httpClient = new HttpClient();
var pollingService = new PollingService(
        new MessageService(token, httpClient),
        new PokeFuseManager(),
        new ImageManager(httpClient),
        new PokeApi(new PokeApiClient(httpClient))
    );

while (true)
{
    await pollingService.PollForUpdatesAsync();
    await Task.Delay(250);
}

