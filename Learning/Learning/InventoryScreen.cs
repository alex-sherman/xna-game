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
        public Rectangle craftRec;
        public Rectangle itemRec;
        public Item[] craftItems = new Item[2];
        public bool inventoryUp = false;

        public InventoryScreen(Player player)
        {
            IsPopup = true;
            this.player = player;
            itemRec.Height = 62;
            itemRec.Width = 62;
            craftRec.Height = 132;
            craftRec.Width = 132;
            craftRec.X = GUI.device.Viewport.Width / 2 - 550;
            craftRec.Y = GUI.device.Viewport.Height / 2 - 200;

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

        #region ItemCrafting
        void HandleCrafting(int X, int Y, bool leftClick){
            if (!leftClick)
            {
                if (X == -1)
                {
                    Item item = craftItems[Y];
                    if (movingItem == null)
                    {
                        if (item.amount == 1)
                        {
                            movingItem = item;
                            craftItems[Y] = null;
                        }
                        else if (item != null)
                        {
                            movingItem = new Item(item.type, item.amount / 2);
                            item.amount -= movingItem.amount;
                        }
                    }
                    else
                    {
                        if (item == null)
                        {
                            craftItems[Y] = new Item(movingItem.type, 1);
                            movingItem.amount--;
                        }
                        else if (item.type == movingItem.type)
                        {
                            item.amount++;
                            movingItem.amount--;
                        }
                        if (movingItem.amount == 0) { movingItem = null; }
                    }
                }
                else
                {
                    //Craft all items
                    Recipe toMake = Crafting.getRecipe(craftItems);
                    if (toMake != null)
                    {
                        int amount = toMake.canCraft(craftItems);
                        if (movingItem == null)
                        {
                            movingItem = Crafting.craft(craftItems, amount);
                        }
                        else if (movingItem.type == toMake.craftedItem.type)
                        {
                            Crafting.craft(craftItems, amount);
                            movingItem.amount += amount;
                        }
                    }
                }
            }
            else
            {
                if (X == -1)
                {
                    Item item = craftItems[Y];
                    if (movingItem == null)
                    {
                        if (item != null)
                        {
                            movingItem = item;
                            craftItems[Y] = null;
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            if (item.type == movingItem.type)
                            {
                                item.amount += movingItem.amount;
                                movingItem = null;
                            }
                        }
                        else
                        {
                            craftItems[Y] = movingItem;
                            movingItem = null;
                        }
                    }
                }
                else
                {
                    Recipe toMake = Crafting.getRecipe(craftItems);
                    if (toMake != null)
                    {
                        if (movingItem == null)
                        {
                            movingItem = Crafting.craft(craftItems, 1);
                        }
                        else if (movingItem.type == toMake.craftedItem.type)
                        {
                            Crafting.craft(craftItems, 1);
                            movingItem.amount += 1;
                        }
                    }
                }
            }
        }
        #endregion

        #region InventoryManagement
        void ManageInventory(int X, int Y, bool leftClick)
        {
            if (leftClick)
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
            else
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
                        movingItem = new Item(item.type, item.amount / 2);
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
                else if(movingItem!=null)
                {
                    player.inventory.items[index] = new Item(movingItem.type, 1);
                    movingItem.amount--;
                    if (movingItem.amount == 0) { movingItem = null; }
                }
            }
        }
        #endregion

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
                bool crafting;
                if (snapMouse(out X, out Y, out crafting))
                {
                    if (crafting)
                    {
                        HandleCrafting(X, Y, true);   
                    }
                    else
                    {
                        ManageInventory(X, Y, true);
                    }
                }
            }
            if (input.IsNewRightClick())
            {
                int X;
                int Y;
                bool crafting;
                if (snapMouse(out X, out Y, out crafting))
                {
                    if (crafting)
                    {
                        HandleCrafting(X, Y, false);
                    }
                    else
                    {
                        ManageInventory(X, Y, false);
                    }
                }
            }
            player.velocity = Vector3.Zero;
            base.HandleInput(input);
        }
        private bool snapMouse(out int X, out int Y, out bool inCrafting)
        {
            X = Mouse.GetState().X - inventoryRec.Left;
            Y = Mouse.GetState().Y - inventoryRec.Top - 90;
            inCrafting = false;
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
            X = Mouse.GetState().X - craftRec.Left;
            Y = Mouse.GetState().Y - craftRec.Top;
            if (X >= 0 && X < craftRec.Width - 2 && Y >= 0 && Y < craftRec.Height - 2)
            {
                if (X < craftRec.Width / 2 - 2)
                {
                    X = -1;
                    Y /= craftRec.Height / 2;
                    inCrafting = true;
                    return true;
                }
                X = 1;
                Y = -1;
                inCrafting = true;
                return true;
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            GUI.drawInventoryHotBar(player.inventory);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(GUI.inventory, inventoryRec, Color.White);
            DrawMenu();
            DrawCrafting();
            if (movingItem != null)
            {

                ScreenManager.SpriteBatch.Draw(
                    Block.textureList[movingItem.type],
                    new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 60, 57),
                    Color.White);
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
        public void DrawCrafting()
        {
            itemRec.X = craftRec.Left;
            itemRec.Y = craftRec.Top;
            ScreenManager.SpriteBatch.Draw(GUI.crafting, craftRec, Color.White);
            if (craftItems[0] != null)
            {
                ScreenManager.SpriteBatch.Draw(Block.textureList[craftItems[0].type], itemRec, Color.White);
                ScreenManager.SpriteBatch.DrawString(
                        GUI.font,
                        craftItems[0].amount.ToString(),
                        new Vector2(itemRec.X, itemRec.Y),
                        Color.Black);
            }
            itemRec.Y += 66;
            if (craftItems[1] != null)
            {
                ScreenManager.SpriteBatch.Draw(Block.textureList[craftItems[1].type], itemRec, Color.White);
                ScreenManager.SpriteBatch.DrawString(
                        GUI.font,
                        craftItems[1].amount.ToString(),
                        new Vector2(itemRec.X, itemRec.Y),
                        Color.Black);
            }
            Recipe recipe = Crafting.getRecipe(craftItems);
            if (recipe != null)
            {
                itemRec.Y -= 33;
                itemRec.X += 66;
                ScreenManager.SpriteBatch.Draw(Block.textureList[recipe.craftedItem.type], itemRec, Color.White);
                ScreenManager.SpriteBatch.DrawString(
                        GUI.font,
                        recipe.canCraft(craftItems).ToString(),
                        new Vector2(itemRec.X, itemRec.Y),
                        Color.Black);
            }

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
