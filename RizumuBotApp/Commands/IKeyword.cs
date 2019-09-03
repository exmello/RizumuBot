using RizumuBot.Model;
using System.Threading.Tasks;

namespace RizumuBot.Commands
{
    public interface IKeyword
    {
        /// <summary>
        /// Processes keywords
        /// </summary>
        /// <param name="input"></param>
        Task Process(MessageInfo message);
    }
}
