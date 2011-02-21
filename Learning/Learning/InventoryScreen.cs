using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Learning
{
    class InventoryScreen : GameScreen
    {
        public Player player;
        public int currentItem = 0;
        public Item movingItem;
        public Rectangle inventoryRec;
        public Rectangle hotBarRec;
        public Rectangle itemRec;
        public bool inventoryUp = false;

        public InventoryScreen(Player player)
        {
            IsPopup = true;
            this.player = player;
            itemRec.Height = 62;
            itemRec.Width = 62;
            inventoryRec.Height = 398;
            inventoryRec.Width = 651;
            inventoryRec.X = GUI.device.Viewport.Width / 2 - 325;
            inventoryRec.Y = GUI.device.Viewport.Height / 2 - 200;
            hotBarRec.Height = 68;
            hotBarRec.Width = 663;
            hotBarRec.X = GUI.device.Viewport.Width / 2 - 331;
            hotBarRec.Y = GUI.device.Viewport.Height - 68;
        }

        public override void LoadContent()
        {
            ScreenManager.Game.IsMouseVisible = true;
            base.LoadContent();
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.I) || input.IsNewKeyPress(Keys.Escape))
            {
                ScreenManager.Game.IsMouseVisible = false;
                ExitScreen();
            }
            
            if (input.IsNewLeftClick())
            {
                int X;
                int Y;
                if (snapMouse(out X, out Y))
                {
                    int index = X + Y * 10 + 10;
                    if (movingItem == null)
                    {
                        GUI.print(X.ToString() + ", " + Y.ToString());
                        Item item = player.inventory.items[index];
                        if (item != null)
                        {
                            movingItem = item;
                            player.inventory.items[index] = null;
                        }
                    }
                    else
                    {
                        Item temp = player.inventory.items[index];
                        if (temp != null && temp.type == movingItem.type)
                        {
                            temp.amount += movingItem.amount;
                            movingItem = null;

                        }
                        else
                        {
                            player.inventory.items[index] = movingItem;
                            movingItem = temp;
                        }
                    }
                }
            }
            if (input.IsNewRightClick())
            {
                int X;
                int Y;
                if (snapMouse(out X, out Y))
                {
                    int index = X + Y * 10 + 10;
                    Item item = player.inventory.items[index];
                    if (movingItem == null && item != null)
                    {
                        if (item.amount == 1)
                        {
                            movingItem = item;
                            player.inventory.items[index] = null;
                        }
                        else
                        {
                            movingItem = new Item(Vector3.Zero, item.type);
                            movingItem.amount = item.amount / 2;
                            item.amount -= movingItem.amount;
                        }
                    }
                    else if (item != null)
                    {
                        if (item.type == movingItem.type)
                        {
                            player.inventory.items[index].amount++;
                            movingItem.amount--;
                            if (movingItem.amount == 0) { movingItem = null; }

                        }
                    }
                    else
                    {
                        player.inventory.items[index] = new Item(new Vector3(0, 0, 0), movingItem.type);
                        movingItem.amount--;
                        if (movingItem.amount == 0) { movingItem = null; }
                    }
                }
            }
            player.velocity = Vector3.Zero;
            base.HandleInput(input);
        }
        private bool snapMouse(out int X, out int Y)
        {
            X = Mouse.GetState().X - inventoryRec.Left;
            Y = Mouse.GetState().Y - inventoryRec.Top - 90;
            if (X >= 0 && X < inventoryRec.Width - 2 && Y >= 0 && Y < inventoryRec.Height - 92)
            {

                X = X * 10 / inventoryRec.Width;
                Y = Y * 5 / (inventoryRec.Height - 90);
                return true;
            }
            X = Mouse.GetState().X - hotBarRec.Left;
            Y = Mouse.GetState().Y - hotBarRec.Top;
            if (X >= 0 && X < hotBarRec.Width - 2 && Y >= 0 && Y < hotBarRec.Height - 2)
            {

                X = X * 10 / hotBarRec.Width;
                Y = -1;
                return true;
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(GUI.inventory, inventoryRec, Color.White);
            DrawMenu();
            if (movingItem != null)
            {
                /*ScreenManager.SpriteBatch.Draw(
                    Block.textureList[movingItem.type],
                    new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                    Color.White);*/

                ScreenManager.SpriteBatch.Draw(
                    Block.textureList[movingItem.type],
                    new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 60, 57),
                    Color.White);
            }
            ScreenManager.SpriteBatch.End();
            GUI.drawInventoryHotBar(player.inventory);
            base.Draw(gameTime);
        }
        private void DrawMenu()
        {
            for (int i = 10; i < 60; i++)
            {
                if (player.inventory.items[i] != null)
                {
                    itemRec.Width = 60;
                    itemRec.Height = 57;
                    itemRec.X = (inventoryRec.Left + 2) + inventoryRec.Width / 10 * (i % 10);
                    itemRec.Y = inventoryRec.Top + 92 + (inventoryRec.Height - 92) / 5 * ((i / 10) - 1);
                    
                    ScreenManager.SpriteBatch.Draw(
                        Block.textureList[player.inventory.items[i].type],
                        itemRec, 
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(
                        GUI.font,
                        player.inventory.items[i].amount.ToString(),
                        new Vector2(itemRec.X, itemRec.Y),
                        Color.Black);
                }
            }
        }
    }
}
