using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning
{
    class Inventory
    {
        public Player player;
        public Item[] items;
        public int currentItem = 0;
        public bool inventoryUp = false;

        public Inventory(Player player)
        {
            this.player = player;
            this.items = new Item[100];


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
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i]!= null && this.items[i].type == toAdd.type)
                {
                    this.items[i].amount+= toAdd.amount;
                    return;
                }
            }
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i] == null)
                {
                    this.items[i] = toAdd;
                    return;
                }
            }
        }
    }
}
