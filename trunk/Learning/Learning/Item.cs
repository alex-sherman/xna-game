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
        public Vector3 Position = Vector3.Zero;
        public bool Visible = false;
        public int type;
        public static List<Item> itemList = new List<Item>();
        public float rotation = 0f;
        public int amount;
        public Item(Vector3 position,int type)
        {
            this.type = type;
            this.Position = position;
            this.amount = 1;
            Item.itemList.Add(this);
            Visible = true;
        }
        public Item(int type,int amount)
        {
            this.type = type;
            this.amount = amount;
            Visible = false;
        }

        public static void Draw(World world)
        {
            foreach (Item item in Item.itemList)
            {
                if (item.Visible)
                {
                    Cube.Draw(item.Position, world, Block.textureList[item.type], .5f, item.rotation);
                }
            }
        }
        public static void Update(World world)
        {
            
            foreach (Item item in Item.itemList)
            {
                item.rotation += .05f;
                if (item.rotation > Math.PI * 2)
                {
                    item.rotation -= 2 * (float)Math.PI;
                }
                foreach (Player player in world.players)
                {
                    if ((player.position - item.Position).Length() < 2.5f)
                    {
                        player.inventory.addItem(item);
                        Item.itemList.Remove(item);
                        return;
                    }
                }
            }
        }
    }
}
