using ChatGPT.Net;
namespace PokeFusionBot.ChatGptFunctions;
public class ChatGptWrapper : IChatGptWrapper {
    private ChatGpt _chatGpt { get; set; }
    public ChatGptWrapper(string token) {
        _chatGpt = new ChatGpt(token);

    }

    public async Task<string> Ask(string prompt) {
        Console.WriteLine("Asking Chatgpt...");
        return await _chatGpt.Ask(prompt);
    }
}