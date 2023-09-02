
public class PollingServiceTests
{
    [Fact]
    public void Test_IdExtractor() {
        var text = "battle 111.222 333.444";
        BattleManager.ExtractIds(text, out int[] firstIds, out int[] secondIds);
        Assert.Equal(111, firstIds[0]);
        Assert.Equal(222, firstIds[1]);
        Assert.Equal(333, secondIds[0]);
        Assert.Equal(444, secondIds[1]);
    }

}