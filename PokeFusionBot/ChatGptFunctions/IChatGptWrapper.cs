namespace PokeFusionBot.ChatGptFunctions;
public interface IChatGptWrapper {
    Task<string> Ask(string prompt);
}