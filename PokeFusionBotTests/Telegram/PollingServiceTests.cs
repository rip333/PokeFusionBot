namespace PokeFusionBotTests.Telegram;

public class PollingServiceTests
{
    [Fact]
    public async void Test_PollForUpdatesAsync()
    {
        List<string> messages = new List<string>() {
            "pikachu pikachu"
        };
        var pokeFuseResponse = new PokeFuseResponse(25, 25, "pikachu.png", "pikachu.png");
        var mockMessageService = GetMockMessageService(messages);
        var mockPokeFuseManager = GetMockPokeFuseManager(pokeFuseResponse);
        var sut = new PollingService(mockMessageService.Object, mockPokeFuseManager.Object);

        // Act
        await sut.PollForUpdatesAsync();

        // Assert
        mockMessageService.Verify(x => x.GetUpdates(It.IsAny<int>()), Times.Once);
        mockPokeFuseManager.Verify(x => x.GetFuseFromMessage("pikachu pikachu"), Times.Once);
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
}