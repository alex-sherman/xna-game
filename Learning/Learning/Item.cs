using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning
{
    class Item
    {
        public Vector3 position;
        public int type;

        public Item(Vector3 position,int type)
        {
            this.type = type;
            this.position = position;
        }
        
    }
}
