#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Learning
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Player someBitch;
        Matrix worldViewProjection;
        static Effect effect;
        MouseState orgMouseState;
        World newWorld = new World();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            
            int[] pointList = {   2, 0, 0,
                                    0, 0, 0,
                                    4, 0, 0,
                                    0, 4, 4,
                                    4, 4, 4,
                                    0, 4, 6,
                                    4, 4, 6,
                                    0, 6, 8,
                                    4, 6, 8,
                                    6,2,2

                                };
            
            // TODO: Add your initialization logic here
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            orgMouseState = Mouse.GetState();
            InitializeTransform();
            Cube.InitializeCube(graphics.GraphicsDevice, InitializeEffect());
            newWorld.addChunk(0, 0, 0);
            newWorld.addChunk(1, 0, 0);
            newWorld.addChunk(-1, 0, 0);
            newWorld.addChunk(0, 0, -2);
            newWorld.addChunk(0, 0, -1);
            someBitch = new Player();
            newWorld.addPlayer(someBitch);
            base.Initialize();
        }

        /// <summary>
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        private void InitializeTransform()
        {
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 2, 10),
                Vector3.Zero, Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4.0f,  // 2 PI Radians is 360 degrees,
                // so this is 45 degrees.
                (float)GraphicsDevice.Viewport.Width /
                (float)GraphicsDevice.Viewport.Height,
                1.0f, 1000.0f);

            worldViewProjection = projection;
        }

        Effect InitializeEffect()
        {
           
            effect = Content.Load<Effect>("ReallySimpleEffect");

            effect.Parameters["WorldViewProj"].SetValue(worldViewProjection);

            effect.CurrentTechnique = effect.Techniques["TransformTechnique"];
            return effect;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Initializes the vertices and indices of the 3D model.
        /// </summary>
        

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            int dx = Mouse.GetState().X - orgMouseState.X;
            int dy = Mouse.GetState().Y - orgMouseState.Y;
            
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            someBitch.Update(gameTime, dx,dy, Keyboard.GetState());
            newWorld.Update(someBitch.getCameraMatrix() * worldViewProjection);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
#if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
#endif

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        void UpdateCamera(GameTime time)
        {
            
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            newWorld.Draw();
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            

            base.Draw(gameTime);
        }
    }
}
