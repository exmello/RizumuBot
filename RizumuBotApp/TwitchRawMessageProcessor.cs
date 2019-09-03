using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RizumuBot.Model;

namespace RizumuBot
{
    public class TwitchRawMessageProcessor
    {
        public MessageInfo GetMessageInfo(string raw)
        {
            string[] message = raw.Split(':');

            if (message.Length < 2)
                throw new ArgumentException($"{nameof(TwitchRawMessageProcessor)}.{nameof(GetMessageInfo)} parameter {nameof(raw)}. Expected ':', found: {raw}");

            string[] preamble = message[1].Split(' ');
            string[] sendingUser = preamble[0].Split('!');

            return new MessageInfo
            {
                Channel = preamble.Length > 2 ? preamble[2].TrimStart('#') : null,
                Action = MapActionType(preamble[1]),
                Username = sendingUser[0],
                Content = message.Length > 2 ? raw.Substring(message[0].Length + message[1].Length + 2) : null
            };
        }

        public MessageActionType MapActionType(string action)
        {
            switch (action)
            {
                // This means it's a message to the channel.  Yes, PRIVMSG is IRC for messaging a channel too
                case "PRIVMSG": return MessageActionType.Message;
                // A user joined.
                case "JOIN": return MessageActionType.Join;
                //  Something else
                default: return MessageActionType.Unknown;
            }
        }
    }
}
