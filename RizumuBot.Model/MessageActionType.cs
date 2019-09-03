using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizumuBot.Model
{
    public enum MessageActionType
    {
        Unknown,
        Join, // "JOIN"
        Part, // "PART"
        Message // "PRIVMSG"
    }
}
