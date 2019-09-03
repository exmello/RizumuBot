
using System.Threading.Tasks;

namespace RizumuBot.Model
{
    public interface ITwitchBot
    {
        Task ProcessMessageAsync(MessageInfo message);
        Task Update();
    }
}
