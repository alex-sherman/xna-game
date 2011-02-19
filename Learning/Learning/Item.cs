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
        public static ArrayList itemList = new ArrayList();
        public float rotation = 0f;
        public int amount;
        public Item(Vector3 position,int type)
        {
            this.type = type;
            this.position = position;
            this.amount = 1;
            Item.itemList.Add(this);
        }
        public static void Draw(World world)
        {
            foreach (Item item in Item.itemList)
            {
                Cube.Draw(item.position, world, Block.textureList[item.type], .5f,item.rotation);
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
                    if ((player.position - item.position).Length() < 2.5f)
                    {
                        player.inventory.addItem(item, 1);
                        Item.itemList.Remove(item);
                        return;
                    }
                }
            }
        }
    }
}
