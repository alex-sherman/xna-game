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
            InitializeTextures();
            SpriteFont font = Content.Load<SpriteFont>("GUIfont");
            Texture2D crosshair = Content.Load<Texture2D>("Texture\\Crosshair");
            Texture2D hotbar = Content.Load<Texture2D>("Texture\\Hotbar");
            GUI.Init(font,graphics.GraphicsDevice,crosshair,hotbar);
            
            Cube.InitializeCube(graphics.GraphicsDevice, InitializeEffect());
            newWorld.addChunk(0, 0, 0);
            Chunk poo = (Chunk)newWorld.chunkList[0];
            newWorld.addChunk(10, 0, 0);
            newWorld.addChunk(-10, 0, 0);
            newWorld.addChunk(0, 0, -20);
            newWorld.addChunk(0, 0, -10);

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
           
            effect = Content.Load<Effect>("TextureEffect");
            effect.Parameters["WorldViewProj"].SetValue(worldViewProjection);
            effect.CurrentTechnique = effect.Techniques["TransformAndTexture"];
            return effect;
        }
        void InitializeTextures()
        {
            Texture2D[] textures = { Content.Load<Texture2D>("Texture\\Dirt"),
                                     Content.Load<Texture2D>("Texture\\Stone"),
                                     Content.Load<Texture2D>("Texture\\Tree"),
                                     Content.Load<Texture2D>("Texture\\leaves")
                                   };
            Block.initTextures(textures);
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
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            newWorld.Draw();
            GUI.Draw(someBitch.inventory);
            base.Draw(gameTime);
        }
    }
}
