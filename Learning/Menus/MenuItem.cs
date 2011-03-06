using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Menus
{
    class MenuItem
    {
        //float selectedAlpha = 1.0f;
        //float unselectedAlpha = 1.0f;
        float currentFade = 1.0f;


        Color selectedColor = Color.Green;
        Color defaultColor = Color.White;

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public bool Pulsate = false;

        public event EventHandler Selected;
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, null);
        }

        public MenuItem(string text)
        {
            Text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isSelected)
                currentFade = Math.Min(currentFade + fadeSpeed, 1);
            else
                currentFade = Math.Max(currentFade - fadeSpeed, 0);
        }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            Color col = isSelected ? selectedColor : defaultColor;

            // handle when the menu screen fades out/in
            col *= screen.TransitionAlpha;

            // handle pulsing menu entries
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulseProgress = (float)Math.Sin(time * 6) + 1;

            // draw the text
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;
            float scale = Pulsate ? 1 + pulseProgress * 0.05f : 1;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            spriteBatch.DrawString(font, Text, Position, col, 0,
                                   origin, scale, SpriteEffects.None, 0);

        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
