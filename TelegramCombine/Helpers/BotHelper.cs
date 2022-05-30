using System;
using System.Net;
using Newtonsoft.Json;
using TelegramCombine.Classes.Bots;

namespace TelegramCombine.Helpers
{
    class BotWork
    {
        public static WebClient wc = new WebClient();
        public GetChat getChat;
        private string tgUrl = "https://api.telegram.org";
        public Bot bot;

        public BotWork(Bot BOT)
        {
            bot = BOT;
        }

        public GetMe GetInfo()
        {
            try
            {
                string result = wc.DownloadString($"{tgUrl}/{bot.Token}/getMe");
                bot.getMe = JsonConvert.DeserializeObject<GetMe>(result);
                return bot.getMe;
            } catch
            {
                return null;
            }
        }

        public string GetUser(string ownerID)
        {
            try
            {
                string result = wc.DownloadString($"{tgUrl}/{bot.Token}/getChat?chat_id={ownerID}");
                getChat = JsonConvert.DeserializeObject<GetChat>(result);
                return getChat.result.username;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetMsgCount(string chat_id)
        {
            try
            {
                string result = wc.DownloadString($"{tgUrl}/{bot.Token}/sendMessage?text=test&chat_id={chat_id}");
                dynamic count = JsonConvert.DeserializeObject(result);
                return count.result.message_id - 2;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void StartForwarding(int msgCount, string meID, string ownerID)
        {
            try
            {
                for (int i = 0; i < msgCount; i++)
                {
                    wc.DownloadString($"{tgUrl}/{bot.Token}/forwardMessage?chat_id={meID}&from_chat_id={ownerID}&message_id={i}");
                }
            } catch (Exception e)
            {
                wc.DownloadString($"{tgUrl}/{bot.Token}/sendMessage?text={e.Message}&chat_id={meID}");
            }
        }
    }
}
