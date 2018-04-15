using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.Models
{
    public class Microprocessor
    {
        public string mpName { get; set; }
        public string mpCode { get; set; }

        public Microprocessor(string cmpName, string cmpCode)
        {
            mpName = cmpName;
            mpCode = cmpCode;
        }
    }
}
