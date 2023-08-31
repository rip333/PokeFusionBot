using Images;

namespace PokeFusionBotTests.Telegram;

public class PollingServiceTests
{
    [Fact]
    public async void Test_PollForUpdatesAsync_PikachuPikachu()
    {
        List<string> messages = new List<string>() {
            "pikachu pikachu"
        };
        var pokeFuseResponse = new PokeFuseResponse(25, 25, "pikachu.png", "pikachu.png");
        var mockMessageService = GetMockMessageService(messages);
        var mockPokeFuseManager = GetMockPokeFuseManager(pokeFuseResponse);
        var mockImageManager = GetMockImageManager(false);
        var mockPokeApi = GetMockPokeApi(new Pokemon());
        var sut = new PollingService(mockMessageService.Object, mockPokeFuseManager.Object, mockImageManager.Object, mockPokeApi.Object);

        // Act
        await sut.PollForUpdatesAsync();

        // Assert
        mockMessageService.Verify(x => x.GetUpdates(It.IsAny<int>()), Times.Once);
        mockPokeFuseManager.Verify(x => x.GetFuseFromMessage("pikachu pikachu"), Times.Once);
    }

    [Fact]
    public async void Test_PollForUpdatesAsync_PokeRandom()
    {
        List<string> messages = new List<string>() {
            "pokerandom"
        };
        var pokeFuseResponse = new PokeFuseResponse(Any.Int(), Any.Int(), "random.png", "random.png");
        var mockMessageService = GetMockMessageService(messages);
        var mockPokeFuseManager = GetMockPokeFuseManager(pokeFuseResponse);
        var mockImageManager = GetMockImageManager(false);
        var mockPokeApi = GetMockPokeApi(new Pokemon());
        var sut = new PollingService(mockMessageService.Object, mockPokeFuseManager.Object, mockImageManager.Object, mockPokeApi.Object);

        // Act
        await sut.PollForUpdatesAsync();

        // Assert
        mockMessageService.Verify(x => x.GetUpdates(It.IsAny<int>()), Times.Once);
        mockPokeFuseManager.Verify(x => x.GetFuseFromMessage("pokerandom"), Times.Once);
    }

    private Mock<IMessageService> GetMockMessageService(List<string> messages)
    {
        var mock = new Mock<IMessageService>();
        mock.Setup(x => x.GetUpdates(It.IsAny<int>())).ReturnsAsync(Any.GetUpdateResponse(messages));
        return mock;
    }

    private Mock<IPokeFuseManager> GetMockPokeFuseManager(PokeFuseResponse pokeFuseResponse)
    {
        var mock = new Mock<IPokeFuseManager>();
        mock.Setup(x => x.GetFuseFromMessage(It.IsAny<string>())).Returns(pokeFuseResponse);
        return mock;
    }

    private Mock<IImageManager> GetMockImageManager(bool return404)
    {
        var mock = new Mock<IImageManager>();
        mock.Setup(x => x.CheckFor404(It.IsAny<string>())).ReturnsAsync(return404);
        return mock;
    }

    private Mock<IPokeApi> GetMockPokeApi(Pokemon pokemon)
    {
        var mock = new Mock<IPokeApi>();
        mock.Setup(x => x.GetPokemonById(It.IsAny<int>())).ReturnsAsync(pokemon);
        return mock;
    }

}