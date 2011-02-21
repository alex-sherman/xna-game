using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Learning.Menus
{
    class ConfirmationDialog : GameScreen
    {
        public delegate void ConfirmationDelegate();
        string prompt;
        string yes = "Yes";
        string no = "No";
        int selected = 0;
        ConfirmationDelegate affirmative, negative;

        public ConfirmationDialog(string prompt,
            ConfirmationDelegate Affirmative,
            ConfirmationDelegate Negative)
        {
            IsPopup = true;
            this.prompt = prompt;
            this.affirmative = Affirmative;
            this.negative = Negative;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GraphicsDevice g = ScreenManager.GraphicsDevice;

            Vector2 dialogPosition = new Vector2(g.Viewport.Width / 4, g.Viewport.Height / 4);
            Vector2 dialogSize = new Vector2(g.Viewport.Width / 2, g.Viewport.Height / 2);
            Rectangle dArea = new Rectangle((int)dialogPosition.X, (int)dialogPosition.Y, 
                                            (int)dialogSize.X, (int)dialogSize.Y);

            Vector2 yesSize = ScreenManager.Font.MeasureString(yes);
            Vector2 noSize = ScreenManager.Font.MeasureString(no);
            Vector2 yesOrigin = yesSize / 2;
            Vector2 noOrigin = noSize / 2;
            Vector2 promptSize = ScreenManager.Font.MeasureString(prompt);
            Vector2 promptOrigin = promptSize / 2;

            Vector2 promptPos = dialogPosition + new Vector2(dialogSize.X / 2, promptSize.Y);
            Vector2 yesPos = dialogPosition + new Vector2(dialogSize.X / 4, 4 * yesSize.Y);
            Vector2 noPos = dialogPosition + new Vector2(3 * dialogSize.X / 4, 4 * noSize.Y);

            Color yesCol = selected == 0 ? Color.Yellow : Color.White;
            Color noCol = selected == 1 ? Color.Yellow : Color.White;

            SpriteBatch sb = ScreenManager.SpriteBatch;
            sb.Begin();
            sb.Draw(ScreenManager.blankTexture, dArea, Color.CornflowerBlue);
            sb.DrawString(ScreenManager.Font, prompt, promptPos, Color.Red, 0,
                          promptOrigin, 1.0f, SpriteEffects.None, 0);
            sb.DrawString(ScreenManager.Font, yes, yesPos, yesCol, 0,
                          yesOrigin, 1.0f, SpriteEffects.None, 0);
            sb.DrawString(ScreenManager.Font, no, noPos, noCol, 0,
                          noOrigin, 1.0f, SpriteEffects.None, 0);

            sb.End();

            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Left))
            {
                selected--;
                if (selected < 0) selected = 1;
            }
            if (input.IsNewKeyPress(Keys.Right))
            {
                selected++;
                if (selected >= 2) selected = 0;
            }
            if (input.IsNewKeyPress(Keys.Enter))
            {
                switch (selected)
                {
                    case 0:
                        affirmative();
                        ExitScreen();
                        break;
                    case 1:
                        negative();
                        ExitScreen();
                        break;
                }
            }
            base.HandleInput(input);
        }
    }
}
