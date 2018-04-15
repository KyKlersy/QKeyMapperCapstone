using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.Models
{
    public class KeyMacro
    {
        public string macroName { get; set; }
        public List<string> macroString { get; set; }

        public KeyMacro()
        {
            macroString = new List<string>();
        }
    }
}
