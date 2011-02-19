using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Learning
{
    class Input
    {
        public Player player;
        private GraphicsDevice device;
        public Input(Player player)
        {
            this.device = World.device;
            this.player = player;
        }

        public void handleInput(GameTime time)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            this.handleRotation(time, mouse);
            this.handleMovement(time, keyboard, mouse);
            this.handleInventory(keyboard, mouse);
        }

        private void handleMovement(GameTime time, KeyboardState keyboard, MouseState mouse)
        {
            
            //Run/walk
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                this.player.speed = GameConstants.PlayerWalkSpeed;
            }
            else
            {
                this.player.speed = GameConstants.PlayerRunSpeed;
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
                this.player.outsideV.Y = .31f;
            }
            
        }
        private void handleInventory(KeyboardState keyboard, MouseState mouse)
        {
            //Select items
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

            //Use items and such
            if (mouse.LeftButton== ButtonState.Pressed&& this.player.actionProgress >= 0)
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
    }
}
