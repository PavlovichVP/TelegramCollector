namespace TelegramCombine.Classes.Bots
{
    public class Info
    {
        public int id { get; set; }
        public bool isbot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public bool can_join_groups { get; set; }
        public bool can_read_all_group_messages { get; set; }
        public bool supports_inline_queries { get; set; }
    }

    class GetMe
    {
        public bool ok { get; set; }
        public Info result { get; set; }
    }
}
