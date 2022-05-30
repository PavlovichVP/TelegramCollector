using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TelegramCombine.Classes.Bots;
using System.IO;
using TeleSharp.TL;
using TelegramCombine.Helpers;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TelegramCombine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("session.dat"))
                File.Delete("session.dat");
        }

        User user = new User();
        Bot bot = new Bot();

        public string BotsForwardDialog_username
        {
            get { return bots_forward_dialog_nickname.Text; }
            set { bots_forward_dialog_nickname.Text = value; }
        }

        public string BotsForwardDialog_name
        {
            get { return bots_forward_dialog_name.Text; }
            set { bots_forward_dialog_name.Text = value; }
        }

        public string BotsForwardDialog_id
        {
            get { return bots_forward_dialog_id.Text; }
            set { bots_forward_dialog_id.Text = value; }
        }

        public string BotsForwardDialog_owner
        {
            get { return bots_forward_dialog_owner.Text; }
            set { bots_forward_dialog_owner.Text = value; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            bot.Token = bots_forward_dialog_token.Text;
            BotWork work = new BotWork(bot);
            work.GetInfo();

            if (work.bot.getMe == null)
                Log.Print(logbox, "Не удалось получить информацию о боте. Скорее всего, токен невалид");

            else
            {
                BotsForwardDialog_owner = $"@{work.GetUser(bots_forward_dialog_ownerid.Text)}";
                BotsForwardDialog_username = $"@{work.bot.getMe.result.username}";
                BotsForwardDialog_name = work.bot.getMe.result.first_name;
                BotsForwardDialog_id = work.bot.getMe.result.id.ToString();

                Log.Print(logbox, $"Была получена информация о боте. " +
                    $"Для пересылки сообщений, отправьте любое сообщение боту: {BotsForwardDialog_username} и нажмите на \"Переслать сообщения\"");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BotWork work = new BotWork(bot);

            if (work.GetMsgCount(bots_forward_dialog_meid.Text) == -1)
                Log.Print(logbox, "Ошибка. Вы точно написали сообщение этому боту?");

            else
            {
                int count = work.GetMsgCount(bots_forward_dialog_meid.Text);
                Log.Print(logbox, $"Было найдено {count} сообщений, начинаю пересылать");
                work.StartForwarding(count, bots_forward_dialog_meid.Text, bots_forward_dialog_ownerid.Text);
                Log.Print(logbox, "Закончил пересылку сообщений");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                user.apiId = Convert.ToInt32(apiID.Text);
                user.apiHash = apiHASH.Text;
                user.phone = chats_parse_phonenumber.Text;

                Log.Print(logbox, "Запрашиваем СМС... Убедитесь, что на аккаунте нет двухэтапной авторизации");
                var telegramClient = new TelegramClient(user.apiId, user.apiHash);
                telegramClient.ConnectAsync().Wait();
                user.hash = telegramClient.SendCodeRequestAsync(user.phone).Result;
            } catch (Exception)
            {
                Log.Print(logbox, "Ошибка при отправке СМС. Попробуйте еще раз");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Log.Print(logbox, "Начинаю парсить чаты");
            ChatWork.ParseChats(user.apiId, user.hash, user.phone, user.hash, user.code);
            Log.Print(logbox, "Чаты были успешно спарсены");
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            try
            {
                user.code = chats_parse_code.Text;
                var telegramClient = new TelegramClient(user.apiId, user.apiHash);
                telegramClient.ConnectAsync().Wait();
                telegramClient.MakeAuthAsync(user.phone, user.hash, user.code).Wait();
                Log.Print(logbox, "Авторизация прошла успешно");
                chats_parse_phonenumber_verif.Text = chats_parse_phonenumber.Text;

                List<TLAbsChat> chats = null;

                TLAbsDialogs dialogs = telegramClient.GetUserDialogsAsync().Result;

                if (dialogs is TLDialogs tldialogs)
                    chats = tldialogs.Chats.ToList();
                
                else if (dialogs is TLDialogsSlice tldialogsSlice)
                    chats = tldialogsSlice.Chats.ToList();

                Log.Print(logbox, $"Найдено {chats.Count} чатов");

                chats_parse_chats_count.Text = chats.Count.ToString();

            }
            catch (Exception)
            {
                Log.Print(logbox, "Проблемы с авторизацией. На аккаунте точно нет двухэтапной защиты?");
            }
        }
    }
}
