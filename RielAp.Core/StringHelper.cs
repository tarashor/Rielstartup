using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Core
{
    public static class StringHelper
    {
        public static string GetOnlyNumbers(string str) {
            return new String(str.Where(Char.IsDigit).ToArray());
        }
    }
}
