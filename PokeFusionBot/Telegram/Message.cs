namespace Telegram;

public class Message
{
    public string Text { get; set; }
    public Chat Chat { get; set; }
    public User? From { get; set; }

}

public class Chat
{
    public long Id { get; set; }
    public string Type { get; set; }
}