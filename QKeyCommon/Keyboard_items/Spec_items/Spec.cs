using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon.Keyboard_items.Spec_items
{
    public class Spec
    {
        public Avrdude avrdude { get; set; }
        public Matrix_spec matrix_spec { get; set; }
        public string diode_direction { get; set; }
    }
}
