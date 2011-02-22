using System;
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
        Matrix worldViewProjection;

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
            
            EnemyAgent.model = Content.Load<Model>("models/ship");

            newWorld = new World(ScreenManager.GraphicsDevice);
            player = new Player();
            newWorld.addPlayer(player);

            InitializeTextures();
            InitializeTransform();
            Cube.InitializeCube(ScreenManager.GraphicsDevice, InitializeEffect());
            GUI.Init(this);

            base.LoadContent();
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
                (float)ScreenManager.GraphicsDevice.Viewport.Width /
                (float)ScreenManager.GraphicsDevice.Viewport.Height,
                0.4f, 2000.0f);

            worldViewProjection = projection;
            newWorld.projection = projection;
        }
        Effect InitializeEffect()
        {
            Effect effect = Content.Load<Effect>("LightAndTextureEffect");
            effect.CurrentTechnique = effect.Techniques["Texture"];
            effect.Parameters["ambientLightColor"].SetValue(
                    Color.White.ToVector4() * .6f);
            effect.Parameters["diffuseLightColor"].SetValue(
                Color.White.ToVector4());
            effect.Parameters["specularLightColor"].SetValue(
                Color.White.ToVector4() / 3);
            effect.Parameters["lightPosition"].SetValue(
                    new Vector3(0f, 10f, 10f));

            effect.Parameters["specularPower"].SetValue(12f);
            effect.Parameters["specularIntensity"].SetValue(.5f);
            return effect;
        }
        void InitializeTextures()
        {
            Texture2D[] textures = { Content.Load<Texture2D>("Textures\\Grass"), //0
                                     Content.Load<Texture2D>("Textures\\Stone"), //1
                                     Content.Load<Texture2D>("Textures\\Wood"), //2
                                     Content.Load<Texture2D>("Textures\\Sand"), //3
                                     Content.Load<Texture2D>("Textures\\leaves"), //4
                                     Content.Load<Texture2D>("Textures\\CraftingTable"), //5
                                     Content.Load<Texture2D>("Textures\\Oven"), //6
                                     Content.Load<Texture2D>("Textures\\IronOre"), //7
                                     Content.Load<Texture2D>("Textures\\Furnace"), //8
                                     Content.Load<Texture2D>("Textures\\IronBlock") //9
                                   };
            Block.initTextures(textures);
        }

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
                newWorld.Update(player.getCameraMatrix());
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
                player.velocity.Z = 1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                player.velocity.Z = -1;
            }
            else { this.player.velocity.Z = 0; }
            if (keyboard.IsKeyDown(Keys.D))
            {
                player.velocity.X = -1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                player.velocity.X = 1;
            }
            else { player.velocity.X = 0; }
            if (player.velocity.LengthSquared() > 0)
            {
                player.velocity.Normalize();
                player.velocity *= player.speed;
            }
            //Jumping
            if (keyboard.IsKeyDown(Keys.Space) && player.isWalking)
            {
                player.outsideV.Y = GameConstants.PlayerJumpSpeed;
                player.isWalking = false;
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

            if (player.yRotation > -MathHelper.PiOver2 && dy < 0 || 
                player.yRotation < MathHelper.PiOver2 && dy > 0)
            {
                player.yRotation += dy * .00175f;
            }
            if (player.xRotation >= 2 * MathHelper.Pi)
            {
                player.xRotation -= 2 * MathHelper.Pi;
            }

            //Use items and such
            if (input.IsNewLeftClick())
            {
                this.player.world.destroyBlock(this.player.lookAt);
            }
            if (input.IsNewRightClick())
            {
                Item item = this.player.inventory.getItem();
                if (item != null)
                {
                    if (this.player.world.addBlock(this.player.lookAt, item.type))
                    {
                        this.player.inventory.useItem();
                    }
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
            newWorld.Draw();
            GUI.drawInventoryHotBar(player.inventory);
            GUI.Draw();
            base.Draw(gameTime);
        }

        #endregion
    }
}
