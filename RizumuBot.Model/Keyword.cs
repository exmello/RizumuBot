using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizumuBot.Model
{
    public class Keyword
    {
        public int ID { get; set; }
        public string Regex { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
    }
}
