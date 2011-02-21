using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Learning
{
    class Inventory
    {
        public Player player;
        public Item[] items;
        public int currentItem = 0;
        public Item movingItem;
        public Rectangle inventoryRec;
        public bool inventoryUp = false;

        public Inventory(Player player)
        {
            this.player = player;
            this.items = new Item[100];
            inventoryRec.Height = 663;
            inventoryRec.Width = 663;
            inventoryRec.X = GUI.device.Viewport.Width/2-331;
            inventoryRec.Y = GUI.device.Viewport.Height / 2 - 329;


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
        public void Draw()
        {
            //Draw the hotbar
            Rectangle curItemRec = new Rectangle();
            curItemRec.Height = 64;
            curItemRec.Width = 65;
            curItemRec.Y = GUI.device.Viewport.Height - 66;
            GUI.batch.Begin();
            GUI.batch.Draw(GUI.hotbar, new Vector2(GUI.device.Viewport.Width / 2 - 331, GUI.device.Viewport.Height - 68), Color.White);
            if (inventoryUp)
            {
                GUI.batch.Draw(GUI.inventory, inventoryRec, Color.White);
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        curItemRec.X = GUI.device.Viewport.Width / 2 + i * 66 - 329;
                        GUI.batch.Draw(Block.textureList[items[i].type], curItemRec, Color.White);
                        GUI.batch.DrawString(GUI.font, items[i].amount.ToString(), new Vector2(GUI.device.Viewport.Width / 2 + i * 66 - 329, GUI.device.Viewport.Height - 64), Color.Black);
                    }
                }
                if (movingItem != null)
                {
                    GUI.batch.Draw(Block.textureList[movingItem.type], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
                }
            }
            else
            {
                //Draw any items in it
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        curItemRec.X = GUI.device.Viewport.Width / 2 + i * 66 - 329;
                        GUI.batch.Draw(Block.textureList[items[i].type], curItemRec, Color.White);
                        GUI.batch.DrawString(GUI.font, items[i].amount.ToString(), new Vector2(GUI.device.Viewport.Width / 2 + i * 66 - 329, GUI.device.Viewport.Height - 64), Color.Black);
                    }
                }
            }
            GUI.batch.End();
        }
    }
}
