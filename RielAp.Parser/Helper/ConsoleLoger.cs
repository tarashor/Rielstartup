using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Helper
{
    public class ConsoleLoger : ILoger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}
