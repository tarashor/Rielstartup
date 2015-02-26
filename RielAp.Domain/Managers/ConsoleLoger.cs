using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RielAp.Domain.Managers
{
    public class ConsoleLoger :ILoger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}
