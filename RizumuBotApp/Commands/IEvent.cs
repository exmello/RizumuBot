using RizumuBot.Model;
using System.Threading.Tasks;

namespace RizumuBot.Commands
{
    public interface IEvent
    {
        /// <summary>
        /// Processes an event
        /// </summary>
        /// <param name="input"></param>
        Task Process(MessageInfo message);
    }
}
