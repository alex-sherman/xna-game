using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning
{
    class Inventory
    {
        private Player player;
        public Item[] items;
        public int[] count;
        public int currentItem = 0;

        public Inventory(Player player)
        {
            this.player = player;
            this.items = new Item[100];
            this.count = new int[100];


        }
        public Item getItem()
        {
            Item toReturn;
            if (count[this.currentItem] == 0) { return null; }
            toReturn = this.items[this.currentItem];
            return toReturn;
        }
        public void useItem()
        {
            if (count[this.currentItem] == 0) { return; }
            this.count[this.currentItem]--;
            if (this.count[this.currentItem] == 0)
            {
                this.items[this.currentItem] = null;
            }
        }
        public void addItem(Item toAdd,int amount)
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i]!= null && this.items[i].type == toAdd.type)
                {
                    this.count[i]+= amount;
                    return;
                }
            }
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i] == null)
                {
                    this.items[i] = toAdd;
                    this.count[i] += amount;
                    return;
                }
            }
        }
    }
}
