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
        MouseState orgMouseState;
        World newWorld;
        Input inputMethod;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1500;
            //graphics.ToggleFullScreen();
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
            newWorld = new World(graphics.GraphicsDevice);
            InitializeTransform();
            InitializeTextures();
            SpriteFont font = Content.Load<SpriteFont>("GUIfont");
            Texture2D crosshair = Content.Load<Texture2D>("Textures\\Crosshair");
            Texture2D hotbar = Content.Load<Texture2D>("Textures\\Hotbar");
            Texture2D inventory = Content.Load<Texture2D>("Textures\\Inventory");
            GUI.Init(font,crosshair,hotbar,inventory);
            
            Cube.InitializeCube(graphics.GraphicsDevice, InitializeEffect());

            someBitch = new Player();
            inputMethod = new Input(someBitch);
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
                0.4f, 2000.0f);

            worldViewProjection = projection;
            newWorld.projection = projection;
        }

        Effect InitializeEffect()
        {
            
            Effect effect = Content.Load<Effect>("LightAndTextureEffect");
            effect.CurrentTechnique = effect.Techniques["Texture"];
            effect.Parameters["ambientLightColor"].SetValue(
                    Color.White.ToVector4()*.6f);
            effect.Parameters["diffuseLightColor"].SetValue(
                Color.White.ToVector4());
            effect.Parameters["specularLightColor"].SetValue(
                Color.White.ToVector4()/3);
            effect.Parameters["lightPosition"].SetValue(
                    new Vector3(0f, 10f, 10f));

            effect.Parameters["specularPower"].SetValue(12f);
            effect.Parameters["specularIntensity"].SetValue(.5f);
            return effect;
        }
        void InitializeTextures()
        {
            Texture2D[] textures = { Content.Load<Texture2D>("Textures\\Grass"),
                                     Content.Load<Texture2D>("Textures\\Stone"),
                                     Content.Load<Texture2D>("Textures\\Wood"),
                                     Content.Load<Texture2D>("Textures\\Sand"),
                                     
                                     Content.Load<Texture2D>("Textures\\leaves")
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

            inputMethod.handleInput(gameTime);
            someBitch.Update(gameTime);
            newWorld.Update(someBitch.getCameraMatrix());
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
            rasterizerState.DepthBias = .01f;
            GraphicsDevice.RasterizerState = rasterizerState;
            newWorld.Draw();
            GUI.Draw(someBitch.inventory);
            base.Draw(gameTime);
        }
    }
}
