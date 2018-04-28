using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.Models
{
    public class MatrixPin
    {
        public string pinName { get; set; }
        //public string pinValue { get; set; }

        public MatrixPin(string cPinName)
        {
            pinName = cPinName;
            //pinValue = cPinValue;
        }
    }
}
