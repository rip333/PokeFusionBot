using Telegram.Bot.Types.InputFiles;
using TelegramBotBase.Base;
using TelegramBotBase.Form;

public class StartForm : FormBase
{
    // Gets invoked during Navigation to this form
    public override async Task PreLoad(MessageResult message)
    {
    }

    // Gets invoked on every Message/Action/Data in this context
    public override async Task Load(MessageResult message)
    {
        // `Device` is a wrapper for current chat - you can easily respond to the user
        var text = message.MessageText;
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
                await Device.SendPhoto(new InputOnlineFile(url));
            }
            catch (Exception)
            {
                return;
            }
        }
    }

    // Gets invoked on edited messages
    public override async Task Edited(MessageResult message)
    {
    }

    // Gets invoked on Button clicks
    public override async Task Action(MessageResult message)
    {
    }

    // Gets invoked on Data uploades by the user (of type Photo, Audio, Video, Contact, Location, Document)
    public override async Task SentData(DataResult data)
    {
    }

    //Gets invoked on every Message/Action/Data to render Design or Response 
    public override async Task Render(MessageResult message)
    {
    }
}