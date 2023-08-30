
namespace Telegram;


public class Chat
{
    public long id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string username { get; set; }
    public string type { get; set; }
    public string title { get; set; }
}

public class EditedMessage
{
    public int message_id { get; set; }
    public From from { get; set; }
    public Chat chat { get; set; }
    public int date { get; set; }
    public int edit_date { get; set; }
    public string text { get; set; }
}

public class From
{
    public long id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string username { get; set; }
}

public class Message
{
    public int message_id { get; set; }
    public From from { get; set; }
    public Chat chat { get; set; }
    public int date { get; set; }
    public string text { get; set; }
    public int? message_thread_id { get; set; }
    public ReplyToMessage reply_to_message { get; set; }
}

public class ReplyToMessage
{
    public int message_id { get; set; }
    public From from { get; set; }
    public Chat chat { get; set; }
    public int date { get; set; }
    public string text { get; set; }
}

public class Result
{
    public int update_id { get; set; }
    public EditedMessage edited_message { get; set; }
    public Message message { get; set; }
}

public class GetUpdateResponse
{
    public bool ok { get; set; }
    public List<Result> result { get; set; }
}
