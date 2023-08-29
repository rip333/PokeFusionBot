namespace Telegram
{
    public class TelegramResponse
    {
        public Update[] Result { get; set; }
    }

    public class Update
    {
        public int Update_id { get; set; }
        public Message Message { get; set; }
    }

}