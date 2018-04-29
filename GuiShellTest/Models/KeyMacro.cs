using System.Collections.Generic;


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
