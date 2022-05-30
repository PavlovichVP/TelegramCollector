namespace TelegramCombine.Classes.Bots
{
    public class Photo
    {
        public string small_file_id { get; set; }
        public string small_file_unique_id { get; set; }
        public string big_file_id { get; set; }
        public string big_file_unique_id { get; set; }
    }

    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
        public string bio { get; set; }
        public Photo photo { get; set; }
    }

    class GetChat
    {
        public bool ok { get; set; }
        public Chat result { get; set; }
    }
}
