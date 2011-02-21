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
        public Rectangle hotBarRec;
        public Rectangle itemRec;
        public bool inventoryUp = false;

        public Inventory(Player player)
        {
            this.player = player;
            this.items = new Item[60];
            itemRec.Height = 62;
            itemRec.Width = 62;
            inventoryRec.Height = 398;
            inventoryRec.Width = 651;
            inventoryRec.X = GUI.device.Viewport.Width/2-325;
            inventoryRec.Y = GUI.device.Viewport.Height / 2 - 200;
            hotBarRec.Height = 68;
            hotBarRec.Width = 663;
            hotBarRec.X = GUI.device.Viewport.Width / 2 - 331;
            hotBarRec.Y = GUI.device.Viewport.Height - 68;


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
        private void DrawHotBar()
        {
            for (int i = 0; i < 10; i++)
            {
                if (items[i] != null)
                {
                    itemRec.Width = 62;
                    itemRec.Height = 62;
                    itemRec.X = GUI.device.Viewport.Width / 2 + i * 66 - 328;
                    GUI.batch.Draw(Block.textureList[items[i].type], itemRec, Color.White);
                    GUI.batch.DrawString(GUI.font, items[i].amount.ToString(), new Vector2(itemRec.X, itemRec.Y), Color.Black);
                }
            }
        }
        private void DrawMenu()
        {
            for (int i = 10; i < 60; i++)
            {
                if (items[i] != null)
                {
                    itemRec.Width = 60;
                    itemRec.Height = 57;
                    itemRec.X = (inventoryRec.Left+2) + inventoryRec.Width / 10 * (i % 10);
                    itemRec.Y = inventoryRec.Top+92 + (inventoryRec.Height -92)/ 5 * ((i / 10)-1);
                    GUI.batch.Draw(Block.textureList[items[i].type], itemRec, Color.White);
                    GUI.batch.DrawString(GUI.font, items[i].amount.ToString(), new Vector2(itemRec.X, itemRec.Y), Color.Black);
                }
            }
        }

        public void Draw()
        {
            //Draw the hotbar
            itemRec.Y = GUI.device.Viewport.Height - 65;
            GUI.batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            GUI.batch.Draw(GUI.hotbar, hotBarRec, Color.White);
            if (inventoryUp)
            {
                GUI.batch.Draw(GUI.inventory, inventoryRec, Color.White);
                DrawHotBar();
                DrawMenu();
                if (movingItem != null)
                {
                    GUI.batch.Draw(Block.textureList[movingItem.type], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
                }
            }
            else
            {
                //Draw any items in it
                DrawHotBar();
            }
            GUI.batch.End();
        }
    }
}
