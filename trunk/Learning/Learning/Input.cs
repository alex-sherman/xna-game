using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Learning
{
    enum InventoryState
    {
        IsWalking,
        EnteringInventory,
        InInventory,
        LeavingInventory
    }

    class Input
    {
        private bool mouseClicked = false;
        public Player player;
        private GraphicsDevice device;
        private InventoryState inventoryState;

        public Input(Player player)
        {
            this.device = World.device;
            this.player = player;
        }

        public void handleInput(GameTime time)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            //this.handleInventory(keyboard, mouse);
            if (inventoryState == InventoryState.IsWalking)
            {
                this.handleRotation(time, mouse);
                this.handleMovement(time, keyboard, mouse);
            }
        }

        private void handleMovement(GameTime time, KeyboardState keyboard, MouseState mouse)
        {

            //Run/walk
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                this.player.speed = GameConstants.PlayerRunSpeed;
            }
            else
            {
                this.player.speed = GameConstants.PlayerWalkSpeed;
            }
            //Movement
            if (keyboard.IsKeyDown(Keys.W))
            {
                this.player.velocity.Z = 1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                this.player.velocity.Z = -1;
            }
            else { this.player.velocity.Z = 0; }
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.player.velocity.X = -1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                this.player.velocity.X = 1;
            }
            else { this.player.velocity.X = 0; }
            if (this.player.velocity.LengthSquared() > 0)
            {
                this.player.velocity.Normalize();
                this.player.velocity *= this.player.speed;
            }
            //Jumping
            if (keyboard.IsKeyDown(Keys.Space) && this.player.isWalking)
            {
                this.player.outsideV.Y = GameConstants.PlayerJumpSpeed;
                player.isWalking = false;
            }

        }

        private void handleRotation(GameTime time, MouseState mouse)
        {
            int dx = Mouse.GetState().X - this.device.Viewport.Width / 2;
            int dy = Mouse.GetState().Y - this.device.Viewport.Height / 2;

            Mouse.SetPosition(this.device.Viewport.Width / 2, this.device.Viewport.Height / 2);
            this.player.xRotation -= dx * .00175f;

            if (this.player.yRotation > -MathHelper.PiOver2 && dy < 0 || this.player.yRotation < MathHelper.PiOver2 && dy > 0)
            {
                this.player.yRotation += dy * .00175f;
            }
            if (this.player.xRotation >= 2 * MathHelper.Pi)
            {
                this.player.xRotation -= 2 * MathHelper.Pi;
            }
        }

        #region Handle Inventory

        private bool snapMouse(out int X, out int Y)
        {
            X = Mouse.GetState().X - player.inventory.inventoryRec.Left;
            Y = Mouse.GetState().Y - player.inventory.inventoryRec.Top - 90;
            if (X >= 0 && X < player.inventory.inventoryRec.Width - 2 && Y >= 0 && Y < player.inventory.inventoryRec.Height - 92)
            {

                X = X * 10 / player.inventory.inventoryRec.Width;
                Y = Y * 5 / (player.inventory.inventoryRec.Height - 90);
                return true;
            }
            X = Mouse.GetState().X - player.inventory.hotBarRec.Left;
            Y = Mouse.GetState().Y - player.inventory.hotBarRec.Top;
            if (X >= 0 && X < player.inventory.hotBarRec.Width - 2 && Y >= 0 && Y < player.inventory.hotBarRec.Height - 2)
            {

                X = X * 10 / player.inventory.hotBarRec.Width;
                Y = -1;
                return true;
            }
            return false;
        }
        private void handleInventory(KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyDown(Keys.I))
            {
                if (inventoryState == InventoryState.IsWalking)
                {
                    inventoryState = InventoryState.EnteringInventory;
                    player.inventory.inventoryUp = true;
                    GUI.game.IsMouseVisible = true;
                }
                if (inventoryState == InventoryState.InInventory)
                {
                    inventoryState = InventoryState.LeavingInventory;
                    player.inventory.inventoryUp = false;
                    GUI.game.IsMouseVisible = false;
                }
            }
            if (keyboard.IsKeyUp(Keys.I))
            {
                if (inventoryState == InventoryState.EnteringInventory)
                {
                    inventoryState = InventoryState.InInventory;
                }
                if (inventoryState == InventoryState.LeavingInventory)
                {
                    Mouse.SetPosition(this.device.Viewport.Width / 2, this.device.Viewport.Height / 2);
                    inventoryState = InventoryState.IsWalking;
                }
            }
            if (player.inventory.inventoryUp)
            {
                if (mouse.LeftButton == ButtonState.Pressed && !mouseClicked)
                {
                    mouseClicked = true;
                    int X;
                    int Y;
                    if (snapMouse(out X, out Y))
                    {
                        int index = X + Y * 10 + 10;
                        if (player.inventory.movingItem == null)
                        {
                            GUI.print(X.ToString() + ", " + Y.ToString());
                            Item item = player.inventory.items[index];
                            if (item != null)
                            {
                                player.inventory.movingItem = item;
                                player.inventory.items[index] = null;
                            }
                        }
                        else
                        {

                            Item temp = player.inventory.items[index];
                            if (temp != null && temp.type == player.inventory.movingItem.type)
                            {
                                temp.amount += player.inventory.movingItem.amount;
                                player.inventory.movingItem = null;

                            }
                            else
                            {
                                player.inventory.items[index] = player.inventory.movingItem;
                                player.inventory.movingItem = temp;
                            }

                        }
                    }
                }
                if (mouse.RightButton == ButtonState.Pressed && !mouseClicked)
                {
                    mouseClicked = true;
                    int X;
                    int Y;
                    if (snapMouse(out X, out Y))
                    {
                        int index = X + Y * 10 + 10;
                        Item item = player.inventory.items[index];
                        if (player.inventory.movingItem == null && item != null)
                        {
                            if (item.amount == 1)
                            {
                                player.inventory.movingItem = item;
                                player.inventory.items[index] = null;
                            }
                            else
                            {
                                player.inventory.movingItem = new Item(Vector3.Zero, item.type);
                                player.inventory.movingItem.amount = item.amount / 2;
                                item.amount -= player.inventory.movingItem.amount;
                            }
                        }
                        else if (item != null)
                        {
                            if (item.type == player.inventory.movingItem.type)
                            {
                                player.inventory.items[index].amount++;
                                player.inventory.movingItem.amount--;
                                if (player.inventory.movingItem.amount == 0) { player.inventory.movingItem = null; }

                            }
                        }
                        else
                        {
                            player.inventory.items[index] = new Item(new Vector3(0, 0, 0), player.inventory.movingItem.type);
                            player.inventory.movingItem.amount--;
                            if (player.inventory.movingItem.amount == 0) { player.inventory.movingItem = null; }
                        }
                    }
                }
                if (mouse.LeftButton == ButtonState.Released && mouse.RightButton == ButtonState.Released)
                {
                    mouseClicked = false;
                }
                player.velocity = Vector3.Zero;
            }

            //Select items with keyboard
            if (inventoryState == InventoryState.IsWalking)
            {
                if (keyboard.IsKeyDown(Keys.D1))
                {
                    this.player.inventory.currentItem = 0;
                }
                if (keyboard.IsKeyDown(Keys.D2))
                {
                    this.player.inventory.currentItem = 1;
                }
                if (keyboard.IsKeyDown(Keys.D3))
                {
                    this.player.inventory.currentItem = 2;
                }
                if (keyboard.IsKeyDown(Keys.D4))
                {
                    this.player.inventory.currentItem = 3;
                }
                if (keyboard.IsKeyDown(Keys.D5))
                {
                    this.player.inventory.currentItem = 4;
                }
                if (keyboard.IsKeyDown(Keys.D6))
                {
                    this.player.inventory.currentItem = 5;
                }
                if (keyboard.IsKeyDown(Keys.D7))
                {
                    this.player.inventory.currentItem = 6;
                }
                if (keyboard.IsKeyDown(Keys.D8))
                {
                    this.player.inventory.currentItem = 7;
                }
                if (keyboard.IsKeyDown(Keys.D9))
                {
                    this.player.inventory.currentItem = 8;
                }
                if (keyboard.IsKeyDown(Keys.D0))
                {
                    this.player.inventory.currentItem = 9;
                }

                //Use items and such
                if (mouse.LeftButton == ButtonState.Pressed && this.player.actionProgress >= 0)
                {
                    this.player.actionProgress = -20;
                    this.player.world.destroyBlock(this.player.lookAt);
                }
                if (mouse.RightButton == ButtonState.Pressed && this.player.actionProgress >= 0)
                {
                    this.player.actionProgress = -20;
                    Item item = this.player.inventory.getItem();
                    if (item != null)
                    {
                        if (this.player.world.addBlock(this.player.lookAt, item.type))
                        {
                            this.player.inventory.useItem();
                        }
                    }
                }
                if (mouse.RightButton == ButtonState.Released && mouse.LeftButton == ButtonState.Released)
                {
                    this.player.actionProgress = 0;
                }
            }
        }

        #endregion
        
        
    }
}
