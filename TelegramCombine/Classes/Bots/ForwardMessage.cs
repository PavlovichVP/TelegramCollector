using System.Collections.Generic;

namespace TelegramCombine.Classes.Bots
{
    public class From
    {
        public int id { get; set; }
        public bool isbot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
    }

    public class UserChat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

    public class ForwardFrom
    {
        public int id { get; set; }
        public bool isbot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
    }

    public class Entity
    {
        public int offset { get; set; }
        public int length { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Document
    {
        public string file_name { get; set; }
        public string mime_type { get; set; }
        public string file_id { get; set; }
        public string file_unique_id { get; set; }
        public int file_size { get; set; }
    }

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public UserChat chat { get; set; }
        public int date { get; set; }
        public ForwardFrom forward_from { get; set; }
        public int forward_date { get; set; }
        public string text { get; set; }
        public Document document { get; set; }
        public List<Entity> entities { get; set; }
    }

    class ForwardMessage
    {
        public bool ok { get; set; }
        public Message result { get; set; }
    }
}
