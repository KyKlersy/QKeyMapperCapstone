using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    class Syntax
    {
        public static string indent(int amount)
        {
            return new string(' ', amount * 4);
        }
    }
}
