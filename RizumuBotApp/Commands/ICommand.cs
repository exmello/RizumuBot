using RizumuBot.Model;
using System.Threading.Tasks;

namespace RizumuBot.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Tests if an input message matches the command
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool IsMatch(MessageInfo message);

        /// <summary>
        /// Processes input
        /// </summary>
        /// <param name="input"></param>
        Task Process(MessageInfo message);
    }
}
