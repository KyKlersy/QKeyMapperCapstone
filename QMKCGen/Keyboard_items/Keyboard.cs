﻿using QMKCGen.Keyboard_items.Key_items;
using QMKCGen.Keyboard_items.Spec_items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.Keyboard_items
{
    class Keyboard
    {
        public Spec spec { get; set; }
        public Key[] keys { get; set; }
        
    }
}
