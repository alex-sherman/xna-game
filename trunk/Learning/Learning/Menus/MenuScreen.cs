using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Menus
{
    class MenuScreen : GameScreen
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        public List<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
        }

        int selectedEntry = 0;
        public string MenuTitle { get; set; }
        bool waitingForInput = false;

        protected delegate void KeyPressHandler(Keys pressedKey);
        KeyPressHandler keyHandlerDelegate;

        public MenuScreen(string menuTitle)
        {
            this.MenuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        protected void catchNextInput(KeyPressHandler handler)
        {
            waitingForInput = true;
            keyHandlerDelegate = handler;
        }

        public override void HandleInput(InputState input)
        {
            if (waitingForInput)
            {
                Keys? lastKey = input.getLastPressedKey();
                if (lastKey != null)
                {
                    keyHandlerDelegate((Keys)lastKey);
                    waitingForInput = false;
                }
                return;
            }
            if (input.IsNewKeyPress(Keys.Up))
            {
                selectedEntry--;
                if (selectedEntry < 0)
                    selectedEntry = menuItems.Count - 1;
            }
            else if (input.IsNewKeyPress(Keys.Down))
            {
                selectedEntry++;
                if (selectedEntry >= menuItems.Count)
                    selectedEntry = 0;
            }

            if (input.IsNewKeyPress(Keys.Enter))
                OnSelectEntry(selectedEntry);

            base.HandleInput(input);
        }

        protected virtual void OnSelectEntry(int selectedIndex)
        {
            menuItems[selectedIndex].OnSelectEntry();
        }

        protected virtual void UpdateMenuItemLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(0f, 175f);

            for (int i = 0; i < menuItems.Count; i++)
            {
                MenuItem menuItem = menuItems[i];
                // center the item horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 -
                             menuItem.GetWidth(this) / 2;

                // be fancy
                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                menuItem.Position = position;

                // and move down for the next entry.
                position.Y += menuItem.GetHeight(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuItemLocations();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();
            for (int i = 0; i < menuItems.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuItems[i].Draw(this, isSelected, gameTime);
            }

            // be fancy
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // title!
            Vector2 titlePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(MenuTitle) / 2;
            Color col = Color.Gray * TransitionAlpha;
            float titleScale = 1.5f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, MenuTitle, titlePosition, col, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
