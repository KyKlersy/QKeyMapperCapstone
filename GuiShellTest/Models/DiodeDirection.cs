using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.Models
{
    public class DiodeDirection
    {
        public string diodeName { get; set; }
        public string diodeValue { get; set; }

        public DiodeDirection(string cdiodeName, string cdiodevalue)
        {
            diodeName = cdiodeName;
            diodeValue = cdiodevalue;
        }
    }
}
