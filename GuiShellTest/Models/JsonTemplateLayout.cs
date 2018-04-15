using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.Models
{
    public class JsonTemplateLayout
    {
        public string layoutName { get; set; }
        public string layoutPath { get; set; }

        public JsonTemplateLayout(string cLayoutName, string cLayoutPath)
        {
            layoutName = cLayoutName;
            layoutPath = cLayoutPath;
        }
    }

}
