using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon.Keyboard_items.Spec_items
{
    public class Matrix_spec
    {
        public int rows { get; set; }
        public int cols { get; set; }
        public List<string> row_pins { get; set; }
        public List<string> col_pins { get; set; }
        public Matrix_spec()
        {
            this.row_pins = new List<string>();
            this.col_pins = new List<string>();
        }
    }
}
