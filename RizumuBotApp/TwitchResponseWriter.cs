using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RizumuBot
{
    public class TwitchResponseWriter
    {

        private readonly Stream _stream;
        private readonly ILog logger;
        private DateTime lastMessageSent = DateTime.MinValue;
        private Queue<string> messageQueue = new Queue<string>();
        private Object syncLock = new Object();

        public TwitchResponseWriter(Stream stream, ILog logger)
        {
            _stream = stream;
            this.logger = logger;
        }

        public async Task RespondMessageAsync(string message)
        {
            //avoid accidental command execution
            if (message != null && message.Length > 0 && message[0] != '.' && message[0] != '/')
            {
                //if the message queue is small, add.  otherwise only add if unique
                //Queue.Contains might be an expensive operation?
                if (messageQueue.Count < 4 || !messageQueue.Contains(message))
                {
                    messageQueue.Enqueue(message);
                }

                await Task.Run(() => RespondFromQueue());
            }
        }

        public void RespondFromQueue()
        {
            lock (syncLock)
            {
                while (messageQueue.Count > 0)
                {
                    //safeguard against bot spam
                    if (lastMessageSent.AddSeconds(2) < DateTime.Now)
                    {
                        string message = messageQueue.Dequeue();

                        string commandText = string.Format("PRIVMSG #{0} :{1}\r\n", Config.ChannelName, message);
                        //string commandText = string.Format("{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{0} :KatBot says {1}\r\n", Config.ChannelName, message);
                        WriteToStream(commandText);

                        logger.DebugFormat("You sent the following message : {0}", commandText);

                        lastMessageSent = DateTime.Now;
                    }

                    
                }
            }
        }

        public void WriteToStream(string text)
        {
            Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteToStreamUnicode(string text)
        {
            Byte[] bytes = System.Text.Encoding.Unicode.GetBytes(text);
            _stream.Write(bytes, 0, bytes.Length);
        }

    }
}
