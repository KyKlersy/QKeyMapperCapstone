using QKeyCommon.Keyboard_items.Key_items;
using QKeyCommon.Keyboard_items.Spec_items;
using QKeyCommon.Keyboard_items.Desc_items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon.Keyboard_items
{
    public class Keyboard
    {
        public UIDesc ui_desc { get; set; }
        public Desc desc { get; set; }
        public Spec spec { get; set; }
        public List<Key> keys { get; set; }
        public Keyboard()
        {
            this.desc = new Desc();
            this.spec = new Spec();
            this.ui_desc = new UIDesc();
        }
    }
}
