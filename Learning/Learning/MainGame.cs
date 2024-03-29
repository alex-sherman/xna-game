﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Learning
{
    class MainGame : GameScreen
    {
        #region Fields

        public ContentManager Content;

        Player player;
        World newWorld;

        float coveredAlpha;

        #endregion

        #region Initialization

        public MainGame()
        {
        }

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");
            Actor.Actor.LoadContent(Content);
            EnemyAgent.model = Content.Load<Model>("models/ship");

            GraphicsEngine.Initialize(ScreenManager,Content);
            newWorld = new World();
            player = new Player();
            newWorld.addPlayer(player);
            GUI.Init(this);
            GraphicsEngine.world = newWorld;
            
            newWorld.chunk = new Landchunk(newWorld,new Vector3(0,0,0),Content.Load<float[]>("Textures\\heightmap"));
            base.LoadContent();
        }

        /// <summary>
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        
       
        #endregion

        #region Updating and Drawing

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (coveredByOtherScreen)
                coveredAlpha = Math.Min(coveredAlpha + 1f / 32, 1);
            else
                coveredAlpha = Math.Max(coveredAlpha - 1f / 32, 0);

            if (IsActive)
            {
                player.Update(gameTime);
                newWorld.Update(gameTime);
            }
        }

        public override void HandleInput(InputState input)
        {
            handleKeyboard(input);
            handleMouse(input);
            if (input.IsNewKeyPress(Keys.I))
            {
                ScreenManager.AddScreen(new InventoryScreen(player));
            }
            if (input.IsNewKeyPress(Keys.Escape))
                ScreenManager.AddScreen(new Menus.PauseScreen(this));
            base.HandleInput(input);
        }

        protected void handleKeyboard(InputState input)
        {
            KeyboardState keyboard = input.CurrentKeyboardState;

            //Save, load game
            if(input.IsNewKeyPress(GameConstants.quickLoadKey)){
                player.world.loadGame("save.sav");
            }
            if(input.IsNewKeyPress(GameConstants.quickSaveKey)){
                player.world.saveGame("save.sav");
            }
            if(input.IsNewKeyPress(Keys.F)){
                GraphicsEngine.wireFrame = !GraphicsEngine.wireFrame;
            }
            if (input.IsNewKeyPress(Keys.E))
            {
                newWorld.merge();
            }
            if (input.IsNewKeyPress(Keys.O))
            {
                Graphics.Settings.enableWater = !Graphics.Settings.enableWater;
            }
            if(input.IsNewKeyPress(Keys.F12)){
                if (Graphics.Settings.waterQuality > 1)
                {
                    Graphics.Settings.setWaterQuality(Graphics.Settings.waterQuality - 1);
                }
                else { Graphics.Settings.setWaterQuality(5); }
            }
            // noclip
            if (input.IsNewKeyPress(Keys.N))
            {
                player.Enabled = !player.Enabled;
            }
            //Run/walk
            if (keyboard.IsKeyDown(Keys.LeftShift))
            {
                player.speed = GameConstants.PlayerRunSpeed;
            }
            else
            {
                player.speed = GameConstants.PlayerWalkSpeed;
            }
            //Movement
            if (keyboard.IsKeyDown(Keys.W))
            {
                player.relativeWalkVelocity.Z = -1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                player.relativeWalkVelocity.Z = 1;
            }
            else { this.player.relativeWalkVelocity.Z = 0; }
            if (keyboard.IsKeyDown(Keys.D))
            {
                player.relativeWalkVelocity.X = 1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                player.relativeWalkVelocity.X = -1;
            }
            else { player.relativeWalkVelocity.X = 0; }
            
            //Jumping
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (player.OnGround)
                {
                    GUI.print("on ground! jumping!");
                    player.LinearVelocity.Y = GameConstants.PlayerJumpSpeed;
                    player.OnGround = false;
                }
                else if (!player.Enabled) //noclip
                {
                    player.relativeWalkVelocity.Y = GameConstants.PlayerJumpSpeed;
                }
            }
            else if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                if (player.OnGround)
                {
                    
                }
                else if (!player.Enabled) //noclip
                {
                    player.relativeWalkVelocity.Y = -GameConstants.PlayerJumpSpeed;
                }
            }
            else { player.relativeWalkVelocity.Y = 0; }
            if (player.relativeWalkVelocity.LengthSquared() > 0)
            {
                player.relativeWalkVelocity.Normalize();
                if (!player.Enabled) //noclip
                {
                    player.relativeWalkVelocity *= player.speed * 10;
                }
                else
                {
                    player.relativeWalkVelocity *= player.speed;
                }
            }
            // select items to use
            if (keyboard.IsKeyDown(Keys.D1))
            {
                player.inventory.currentItem = 0;
            }
            if (keyboard.IsKeyDown(Keys.D2))
            {
                player.inventory.currentItem = 1;
            }
            if (keyboard.IsKeyDown(Keys.D3))
            {
                player.inventory.currentItem = 2;
            }
            if (keyboard.IsKeyDown(Keys.D4))
            {
                player.inventory.currentItem = 3;
            }
            if (keyboard.IsKeyDown(Keys.D5))
            {
                player.inventory.currentItem = 4;
            }
            if (keyboard.IsKeyDown(Keys.D6))
            {
                player.inventory.currentItem = 5;
            }
            if (keyboard.IsKeyDown(Keys.D7))
            {
                player.inventory.currentItem = 6;
            }
            if (keyboard.IsKeyDown(Keys.D8))
            {
                player.inventory.currentItem = 7;
            }
            if (keyboard.IsKeyDown(Keys.D9))
            {
                player.inventory.currentItem = 8;
            }
            if (keyboard.IsKeyDown(Keys.D0))
            {
                player.inventory.currentItem = 9;
            }
        }
        protected void handleMouse(InputState input)
        {
            MouseState mouse = input.CurrentMouseState;

            int dx = mouse.X - ScreenManager.GraphicsDevice.Viewport.Width / 2;
            int dy = mouse.Y - ScreenManager.GraphicsDevice.Viewport.Height / 2;
            
            Mouse.SetPosition(
                ScreenManager.GraphicsDevice.Viewport.Width / 2,
                ScreenManager.GraphicsDevice.Viewport.Height / 2);

            player.xRotation -= dx * .00175f;

            if (player.yRotation > -MathHelper.PiOver2 && dy > 0 || 
                player.yRotation < MathHelper.PiOver2 && dy < 0)
            {
                player.yRotation -= dy * .00175f;
            }
            if (player.xRotation >= 2 * MathHelper.Pi)
            {
                player.xRotation -= 2 * MathHelper.Pi;
            }

            //Use items and such
            
            if (input.IsNewRightClick())
            {
                Item item = this.player.inventory.getItem();
                if (item != null)
                {
                    
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.DepthBias = .01f;
            ScreenManager.GraphicsDevice.RasterizerState = rasterizerState;
            GUI.gameTime = gameTime.ElapsedGameTime.Ticks;
            newWorld.Draw();
            GUI.timeDifference = gameTime.ElapsedGameTime.Ticks - GUI.gameTime;
            GUI.drawInventoryHotBar(player.inventory);
            GUI.Draw();
            base.Draw(gameTime);
        }

        #endregion
    }
}
