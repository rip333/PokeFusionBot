using Images;
using PokeApiNet;
using Pokemon;
using Telegram;

Console.WriteLine("PokeFusionBot, I choose you!");

var pokeClient = new PokeApiClient();
var token = await ConfigUtility.GetToken();
var httpClient = new HttpClient();
var pollingService = new PollingService(new MessageService(token, httpClient), new PokeFuseManager(), new ImageManager(httpClient));

while (true)
{
    await pollingService.PollForUpdatesAsync();
    await Task.Delay(250);
}

