namespace RizumuBotApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;
    using RizumuBot;
    using RizumuBot.Model;
    using RizumuBot.Commands;
    using RizumuBot.VRChat.OSC;

    internal class Main
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal static async Task Work(string[] args)
        {
            await Task.FromResult(0);

            Logger.Info("Hello exmello! CLR:4.0.30319.42000");

            1.UpTo(8).ForEach(i => Logger.Debug("_".JoinArray("^".Times(i))));

            // Get a client stream for reading and writing.
            using (TwitchChatConnection connection = new TwitchChatConnection(Config.ChatServer, Config.Port, Logger))
            {
                // Send login request
                Logger.Info("Sent login.\r\n");
                string responseData = connection.SendLoginRequest(Config.OAuth, Config.Nickname);

                if(!connection.IsLoginSuccessful(responseData))
                {
                    Logger.ErrorFormat("Failed to login {0}", responseData);
                    return;
                }

                Logger.InfoFormat("Received WELCOME: \r\n\r\n{0}", responseData);

                // send message to join channel
                connection.JoinChannel(Config.ChannelName);
                Logger.Info("Sent channel join.\r\n");

                // subscribe to events
                responseData = connection.SubcribeToMembershipEvents(Config.ChannelName);
                Logger.InfoFormat("Subcribe to JOIN/PART: \r\n\r\n{0}", responseData);

                //twitch JSON api for bots to use
                //TwitchApiClient api = new TwitchApiClient();

                //osc client for vrc
                VrcOscSender osc = new VrcOscSender(Logger);

                //db storage
                //IViewerRepository viewerDb = new SqlViewerRepository();

                //start bot   
                RizumuBot rizumuBot = new RizumuBot(connection.Writer);//, api);

                //add commands
                rizumuBot.CommandList.Add(new CameraCommand(connection.Writer, osc));

                //Start message loop
                while (true)
                {
                    MessageInfo message = connection.ReadMessage();

                    if (message != null)
                    {
                        await rizumuBot.ProcessMessageAsync(message);
                    }
                }

            }

        }
    }
}
