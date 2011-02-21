using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Learning
{
    class Inventory
    {
        public Item[] items;
        public int currentItem = 0;

        public Inventory()
        {
            this.items = new Item[60];
        }

        public Item getItem()
        {
            Item toReturn;
            toReturn = this.items[this.currentItem];
            return toReturn;
        }

        public void useItem()
        {
            Item item = this.items[this.currentItem];
            if (item.amount == 0) { return; }
            item.amount--;
            if (item.amount == 0)
            {
                this.items[this.currentItem] = null;
            }
        }

        public void addItem(Item toAdd)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].type == toAdd.type)
                {
                    items[i].amount += toAdd.amount;
                    return;
                }
            }
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = toAdd;
                    return;
                }
            }
        }
    }
}
