using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon.Keyboard_items.Key_items
{
    public class Binding
    {
        public List<string> on_tap;
        public List<string> on_hold;
        public Binding()
        {
            this.on_hold = new List<string>();
            this.on_tap = new List<string>();
        }
    }
}
