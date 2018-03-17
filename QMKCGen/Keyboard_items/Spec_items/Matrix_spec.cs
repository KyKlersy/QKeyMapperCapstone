using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.Keyboard_items.Spec_items
{
    class Matrix_spec
    {
        public int rows { get; set; }
        public int cols { get; set; }
        public string[] row_pins { get; set; }
        public string[] col_pins { get; set; }
    }
}
