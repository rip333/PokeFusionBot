using Microsoft.Extensions.Configuration;

public class ConfigUtility
{
    public static async Task<BotConfig> GetBotConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
        var config = builder.Build();
        var botConfig = new BotConfig();
        var telegramTokenLocation = config["AppSettings:TelegramTokenFileLocation"];
        botConfig.TelegramToken = (await File.ReadAllTextAsync(telegramTokenLocation)).Trim();
        var chatGptFileLocation = config["AppSettings:OpenAiTokenFileLocation"];
        botConfig.ChatGptToken = (await File.ReadAllTextAsync(chatGptFileLocation)).Trim();
        return botConfig;
    }
}

public class BotConfig {
    public string TelegramToken { get; set; }
    public string ChatGptToken { get; set; }
}