using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RizumuBot.Commands;
using RizumuBot.Model;
//using RizumuBot.TwitchApi;

namespace RizumuBot
{
    public class RizumuBot : ITwitchBot
    {
        private readonly TwitchResponseWriter tw;
        //private readonly TwitchApiClient api;
        private readonly string[] ignoreBots;

        public IList<ICommand> CommandList { get; set; }
        public IList<IEvent> EventList { get; set; }
        public IList<IKeyword> KeywordProcessors { get; set; }

        public RizumuBot(TwitchResponseWriter tw)//, TwitchApiClient api)
        {
            this.tw = tw;
            //this.api = api;
            this.ignoreBots = new string[] { "moobot", "nightbot", "whale_bot" };
            
            CommandList = new List<ICommand>();
            EventList = new List<IEvent>();
            KeywordProcessors = new List<IKeyword>();

            // Lets you know its working
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            tw.RespondMessageAsync("MrDestructoid MrDestructoid MrDestructoid MrDestructoid MrDestructoid MrDestructoid MrDestructoid");
            tw.RespondMessageAsync("test");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public async Task ProcessMessageAsync(MessageInfo message)
        {
            if (message.Action == MessageActionType.Message)
            {
                // Ignore some well known bots
                if (!ignoreBots.Contains(message.Username.ToLowerInvariant()))
                {
                    //Act on message content
                    await ProcessChatMessageAsync(message);
                }
            }
            else
            {
                await RespondToEvents(message);
            }
        }

        public async Task ProcessChatMessageAsync(MessageInfo message)
        {
            if (!(Config.IgnoreSelf && message.Username.Equals(Config.Nickname, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (message.Content.StartsWith("!"))
                    await RespondToCommandsAsync(message);

                await RespondToKeywordsAsync(message);
            }
        }

        private async Task RespondToCommandsAsync(MessageInfo message)
        {
            foreach (ICommand command in CommandList.OfType<ICommand>())
            {
                if(command.IsMatch(message))
                {
                    await command.Process(message);
                    return;
                }
            }
        }

        private async Task RespondToEvents(MessageInfo message)
        {
            foreach (IEvent evnt in CommandList.OfType<IEvent>())
            {
                await evnt.Process(message);
            }
        }

        private async Task RespondToKeywordsAsync(MessageInfo message)
        {
            foreach (IKeyword keyword in KeywordProcessors)
            {
                await keyword.Process(message);
            }
        }

        private async Task ProcessJoinEvent(string username)
        {
            await tw.RespondMessageAsync(string.Format("Welcome {0}!", username));
        }

        private void BanUser(string username)
        {
            //implement
        }

        private void TimeoutUser(string username, int timeout)
        {
            //implement
        }

        public async Task Update()
        {
            await Task.Yield();
            //implement
        }
    }
}
