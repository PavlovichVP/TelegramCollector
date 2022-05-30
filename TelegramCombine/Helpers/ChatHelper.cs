using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using TelegramCombine.Classes.Channels;
using TLChatFull = TeleSharp.TL.Messages.TLChatFull;

namespace TelegramCombine.Helpers
{
    class ChatWork
    {
        public static ChannelInfo GetChatInfo(TelegramClient client, TLChannel channel)
        {
            try
            {
                ChannelInfo result = new ChannelInfo();
                result.Channel = channel;
                var req = new TLRequestGetFullChannel()
                {
                    Channel = new TLInputChannel() { AccessHash = channel.AccessHash.Value, ChannelId = channel.Id }
                };

                var res = client.SendRequestAsync<TLChatFull>(req).Result;

                var offset = 0;
                result.ChatFull = res;
                while (offset < (res.FullChat as TLChannelFull).ParticipantsCount)
                {
                    var pReq = new TLRequestGetParticipants()
                    {
                        Channel = new TLInputChannel() { AccessHash = channel.AccessHash.Value, ChannelId = channel.Id },
                        Filter = new TLChannelParticipantsRecent() { },
                        Limit = 200,
                        Offset = offset
                    };
                    var pRes = client.SendRequestAsync<TLChannelParticipants>(pReq).Result;
                    result.Users.AddRange(pRes.Users.Cast<TLUser>());
                    offset += 200;
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void ParseChats(int apiID, string apiHash, string phone, string hash, string code)
        {
            try
            {
                var telegramClient = new TelegramClient(apiID, apiHash);
                telegramClient.ConnectAsync().Wait();
                telegramClient.MakeAuthAsync(phone, hash, code).Wait();

                List<TLAbsChat> chats = null;

                TLAbsDialogs dialogs = telegramClient.GetUserDialogsAsync().Result;

                if (dialogs is TLDialogs tldialogs)
                {
                    chats = tldialogs.Chats.ToList();
                }
                else if (dialogs is TLDialogsSlice tldialogsSlice)
                {
                    chats = tldialogsSlice.Chats.ToList();
                }

                var channels = chats.Where(c => c.GetType() == typeof(TLChannel)).Cast<TLChannel>();

                List<ChannelInfo> result = new List<ChannelInfo>();

                foreach (var channel in channels)
                {
                    var res = GetChatInfo(telegramClient, channel);

                    if (res != null)
                    {
                        result.Add(res);
                    }
                }

                DirectoryInfo path = Directory.CreateDirectory(
                    DateTime.Now.ToString("dd-MM-yyyy-HH-mm"));

                foreach (var r in result)
                {
                    try
                    {
                        File.WriteAllText($@"{path}\{r.Channel.Title}.txt", string.Join(Environment.NewLine,
                            r.Users.Select(x => $"{(string.IsNullOrEmpty(x.Username) ? x.FirstName + " " + x.LastName : $"@{x.Username}")} ({x.Id})")));
                    }
                    catch (Exception)
                    {

                    }
                }
            } catch (Exception)
            {
                
            }
        }
    }
}
