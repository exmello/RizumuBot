using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RizumuBot.Model;

namespace RizumuBot.Commands
{
    /// <summary>
    /// Responds to !command or !cmd with a list of valid commands
    /// </summary>
    public class Command : ICommand
    {
        private readonly TwitchResponseWriter tw;
        private readonly Regex regCommand;

        public Command(TwitchResponseWriter tw)
        {
            this.tw = tw;
            this.regCommand = new Regex("^!(command|cmd)\\s$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(MessageInfo message)
        {
            return regCommand.IsMatch(message.Content);
        }

        public async Task Process(MessageInfo message)
        {
            await tw.RespondMessageAsync("Commands: !howlong [user], !uptime [channel], !viewers, !madlib [template] (template keywords: noun/adjective/verb/adverb)");
        }
    }
}
