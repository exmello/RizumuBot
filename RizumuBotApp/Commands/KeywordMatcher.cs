//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using RizumuBot.Model;

//namespace RizumuBot.Commands
//{
//    /// <summary>
//    /// Responds to keywords based on regex matches to settings data
//    /// </summary>
//    public class KeywordMatcher : IKeyword
//    {
//        private readonly TwitchResponseWriter tw;
//        private readonly IChannelRepository repo;

//        private IList<KeywordRegex> keywordRegex = null;

//        public KeywordMatcher(TwitchResponseWriter tw, IChannelRepository repo)
//        {
//            this.tw = tw;
//            this.repo = repo;

//            LoadKeywords();
//        }

//        public bool IsMatch(MessageInfo message)
//        {
//            return false;
//        }

//        void IKeyword.Process(MessageInfo message)
//        {
//            string response = string.Empty;

//            //find match by username
//            KeywordRegex match = keywordRegex.FirstOrDefault(k =>
//                k.Keyword.Username != null &&
//                k.Keyword.Username.ToLowerInvariant() == message.Username.ToLowerInvariant() &&
//                k.Regex.IsMatch(message.Content));

//            if(match == null)
//            {
//                match = keywordRegex.FirstOrDefault(k =>
//                k.Keyword.Username == null &&
//                k.Regex.IsMatch(message.Content));
//            }

//            if (match != null)
//            {
//                tw.RespondMessage(match.Keyword.Message);
//            }
//        }

//        private void LoadKeywords()
//        {
//            var keywords = repo.GetAllKeywords();

//            if (keywords != null)
//            {
//                keywordRegex = keywords.Select(k => new KeywordRegex
//                {
//                    Keyword = k,
//                    Regex = new Regex(k.Regex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
//                }).ToList();
//            }
//        }

//        internal class KeywordRegex
//        {
//            public Keyword Keyword { get; set; }
//            public Regex Regex { get; set; }
//        }
//    }
//}
