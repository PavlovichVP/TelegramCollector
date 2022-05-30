namespace TelegramCombine.Classes.Bots
{
    class Bot
    {
        private string _token;
        public string Token
        {
            get { return _token; }
            set
            {
                if (!value.StartsWith("bot"))
                    _token = "bot" + value;
                else
                    _token = value;
            }
        }

        public GetMe getMe { get; set; }

    }
}
