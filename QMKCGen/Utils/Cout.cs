using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.Utils
{
    class Cout
    {
        public static void printf(string format_string, params object[] args)
        {
            Console.WriteLine(string.Format(format_string, args));
        }
    }
}
