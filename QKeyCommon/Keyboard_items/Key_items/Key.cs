﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon.Keyboard_items.Key_items
{
    public class Key
    {
        public Graphics graphics;
        public Binding binding;
        public Matrix matrix;
        public Key()
        {
            this.graphics = new Graphics();
            this.binding = new Binding();
            this.matrix = new Matrix();

        }

    }
}
