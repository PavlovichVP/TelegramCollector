using System.Collections.Generic;
using TeleSharp.TL;

namespace TelegramCombine.Classes.Channels
{
    class ChannelInfo
    {
        public TLChannel Channel { get; set; }
        public List<TLUser> Users { get; set; } = new List<TLUser>();
        public TeleSharp.TL.Messages.TLChatFull ChatFull { get; set; }
    }
}
