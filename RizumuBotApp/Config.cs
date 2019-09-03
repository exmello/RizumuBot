using Microsoft.Extensions.Configuration;
using System;

namespace RizumuBot
{
    public static class Config
    {
        private static IConfigurationRoot _configuration;

        internal static void Init(IConfigurationRoot configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _configuration = configuration;
        }

        private static string GetEnvironmentVariable(string key) => _configuration["Values:" + key];

        private static string _channelName = null;
        public static string ChannelName 
        { 
            get
            {
                if (_channelName == null)
                    _channelName = GetEnvironmentVariable("TwitchBot.ChannelName");
                return _channelName;
            }
        }

        //Convert your password to OAuth using this link: https://twitchapps.com/tmi/
        private static string _oath = null;
        public static string OAuth
        {
            get
            {
                if (_oath == null)
                    _oath = GetEnvironmentVariable("TwitchBot.OAuth");
                return _oath;
            }
        }

        private static string _nickname = null;
        public static string Nickname
        {
            get
            {
                if (_nickname == null)
                    _nickname = GetEnvironmentVariable("TwitchBot.Nickame");
                return _nickname;
            }
        }

        private static string _botname = null;
        public static string BotName
        {
            get
            {
                if (_botname == null)
                    _botname = GetEnvironmentVariable("TwitchBot.BotName");
                return _botname;
            }
        }

        private static string _chatServer = null;
        public static string ChatServer
        {
            get
            {
                if (_chatServer == null)
                    _chatServer = GetEnvironmentVariable("TwitchBot.ChatServer");
                return _chatServer;
            }
        }

        private static int? _port;
        public static int Port
        {
            get
            {
                if (!_port.HasValue)
                    _port = int.Parse(GetEnvironmentVariable("TwitchBot.Port"));
                return _port.Value;
            }
        }

        private static bool? _ignoreSelf;
        public static bool IgnoreSelf
        {
            get
            {
                if (!_ignoreSelf.HasValue)
                    _ignoreSelf = bool.Parse(GetEnvironmentVariable("TwitchBot.IgnoreSelf"));
                return _ignoreSelf.Value;
            }
        }

        private static string _oscServer = null;
        public static string OscServer
        {
            get
            {
                if (_oscServer == null)
                    _oscServer = GetEnvironmentVariable("TwitchBot.OscServer");
                return _oscServer;
            }
        }

        private static int? _oscPort;
        public static int OscPort
        {
            get
            {
                if (!_oscPort.HasValue)
                    _oscPort = int.Parse(GetEnvironmentVariable("TwitchBot.OscPort"));
                return _oscPort.Value;
            }
        }
    }
}
