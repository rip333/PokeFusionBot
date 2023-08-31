namespace PokeFusionBotTests;

public class Any
{
    private static readonly Random _random = new();

    public static string String(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public static int Int(int min = 0, int max = 1000)
    {
        return _random.Next(min, max);
    }

    public static GetUpdateResponse GetUpdateResponse(List<string> messages)
    {
        var results = new List<Result>();
        for (int i = 0; i < messages.Count; i++)
        {
            results.Add(Result(messages[i]));
        }

        return new GetUpdateResponse()
        {
            ok = true,
            result = results
        };
    }

    public static Result Result(string messageText)
    {
        return new Result()
        {
            update_id = Int(),
            message = Message(messageText)
        };
    }

    public static Chat Chat()
    {
        return new Chat()
        {
            id = Int(),
            first_name = String(),
            last_name = String(),
            username = String(15),  // username typically shorter
            type = String(),
            title = String()
        };
    }

    public static EditedMessage EditedMessage()
    {
        return new EditedMessage()
        {
            message_id = Int(),
            from = From(),
            chat = Chat(),
            date = Int(),
            edit_date = Int(),
            text = String(50)  // assume messages might be a bit longer
        };
    }

    public static From From()
    {
        return new From()
        {
            id = Int(),
            first_name = String(),
            last_name = String(),
            username = String(15)
        };
    }

    public static Message Message(string text)
    {
        return new Message()
        {
            message_id = Int(),
            from = From(),
            chat = Chat(),
            date = Int(),
            text = text,
            message_thread_id = _random.Next(0, 2) == 0 ? (int?)Int() : null,  // 50% chance to have a thread ID
            reply_to_message = _random.Next(0, 2) == 0 ? ReplyToMessage() : null  // 50% chance to have a reply
        };
    }

    public static ReplyToMessage ReplyToMessage()
    {
        return new ReplyToMessage()
        {
            message_id = Int(),
            from = From(),
            chat = Chat(),
            date = Int(),
            text = String(50)
        };
    }
}