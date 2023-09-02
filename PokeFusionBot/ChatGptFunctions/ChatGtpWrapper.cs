using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
namespace PokeFusionBot.ChatGptFunctions;
public class ChatGptWrapper : IChatGptWrapper
{
    private ChatGpt _chatGpt { get; set; }
    public ChatGptWrapper(string token)
    {
        ChatGptOptions chatGptOptions = new ChatGptOptions() {
            MaxTokens = 512L
        };
        _chatGpt = new ChatGpt(token, chatGptOptions);

    }

    public async Task<string> Ask(string prompt)
    {
        Console.WriteLine("Asking Chatgpt...");
        return await _chatGpt.Ask(prompt);
    }
}