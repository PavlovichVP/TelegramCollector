using System.Windows.Forms;

namespace TelegramCombine.Helpers
{
    class Log
    {
        public static void Print(TextBox log, string text)
        {
            log.Text += $"\r\n{text}. ";
        }
    }
}
