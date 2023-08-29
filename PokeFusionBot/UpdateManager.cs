using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class UpdateManager
{
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;
        if (update.Message != null && update.Message.Type != MessageType.Text)
            return;
        if (update.Message == null)
            return;

        var chatId = update.Message.Chat.Id;

        Console.WriteLine($"Received message in chat {chatId}.");

        var text = update.Message.Text;
        var splitString = text.Split(" ");
        if (splitString.Length > 3) return;

        Console.WriteLine(text);
        var matches = new int[2];
        int i = 0;
        foreach (var word in splitString)
        {
            if (i == 2) break;
            if (PokeData.dict.ContainsKey(word.ToLower()))
            {
                int id = PokeData.dict[word.ToLower()];
                if (id != 0)
                {
                    matches[i] = PokeData.dict[word.ToLower()];
                    i++;
                }
            }
        }

        if (matches.Length == 2)
        {
            var url = await PokeFuseManager.GetUrlsFromPokemonIds(matches);
            try
            {
                await botClient.SendPhotoAsync(chatId: chatId, photo: url,
                           cancellationToken: cancellationToken);
            }
            catch(Exception) {
                return;
            }
        }


    }
}