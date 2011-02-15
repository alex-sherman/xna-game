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
        const int number_of_vertices = 24;
        const int number_of_indices = 36;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            orgMouseState = Mouse.GetState();
            InitializeTransform();
            InitializeEffect();
            Block.InitializeCube(graphics);

            Block.addBlock(new Vector3(1.0f, 0.0f, 0.0f));
            Block.addBlock(new Vector3(0.0f, -1.0f, 0.0f));
            Block.addBlock(new Vector3(0.0f, 0.0f, 0.0f));
            Block.addBlock(new Vector3(-1.0f, 0.0f, 0.0f));
            someBitch = new Player();
            base.Initialize();
        }

        /// <summary>
        /// Initializes the transforms used for the 3D model.
        /// </summary>
        private void InitializeTransform()
        {
            float tilt = (float)Math.PI / 8.0f;
           

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

        void InitializeEffect()
        {
           
            effect = Content.Load<Effect>("ReallySimpleEffect");

            effect.Parameters["WorldViewProj"].SetValue(worldViewProjection);

            effect.CurrentTechnique = effect.Techniques["TransformTechnique"];
            Block.effect = effect;
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

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            Block.Draw(GraphicsDevice, someBitch.getCameraMatrix()*worldViewProjection,someBitch.getCameraPos());
            

            base.Draw(gameTime);
        }
    }
}
