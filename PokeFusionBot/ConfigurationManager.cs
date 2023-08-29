using Microsoft.Extensions.Configuration;

public class ConfigUtility
{
    public static async Task<string> GetToken()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
        var config = builder.Build();
        var tokenFileLocation = config["AppSettings:TokenFileLocation"];
        Console.WriteLine(tokenFileLocation);
        var token = await File.ReadAllTextAsync(tokenFileLocation);
        Console.WriteLine(token);
        return token;
    }
}