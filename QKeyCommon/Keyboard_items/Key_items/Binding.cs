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

        public bool is_macro()
        {
            return (on_tap.Count() > 1 || on_hold.Count() > 1);
        }

        public bool is_multifunction()
        {
            return (on_hold.Count() + on_tap.Count() > 1);
        }
    }
}
