using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Learning
{
    class GUI
    {
        public static SpriteFont font;
        public static GraphicsDevice device;
        public static MainGame game;
        public static SpriteBatch batch;
        private static ArrayList strings = new ArrayList();
        private static Texture2D crosshair;
        public static Texture2D hotbar;
        public static float gameTime = 0;
        public static float timeDifference = 0;
        public static Texture2D inventory;
        public static Texture2D crafting;
        private static int lineNumber = 0;
        public static Rectangle hotBarRec, itemRec;

        public static void Init(MainGame game)
        {
            GUI.game = game;
            GUI.font = game.Content.Load<SpriteFont>("GUIfont");
            GUI.crosshair = game.Content.Load<Texture2D>("Crosshair");
            GUI.hotbar = game.Content.Load<Texture2D>("Hotbar");
            GUI.inventory = game.Content.Load<Texture2D>("Inventory");
            GUI.crafting = game.Content.Load<Texture2D>("InvCraft");
            GUI.device = World.device;
            GUI.batch = game.ScreenManager.SpriteBatch;

            hotBarRec.Height = 68;
            hotBarRec.Width = 663;
            hotBarRec.X = GUI.device.Viewport.Width / 2 - 331;
            hotBarRec.Y = GUI.device.Viewport.Height - 68;
            itemRec.Height = 62;
            itemRec.Width = 62;
        }

        public static void print(String toPrint)
        {
            if (GUI.strings.Count > 10) { GUI.strings.RemoveAt(0); }
            GUI.lineNumber++;
            GUI.strings.Add(GUI.lineNumber.ToString() + ": " + toPrint);
        }

        public static void Draw()
        {
            //Draw the cursor in additive mode
            GUI.batch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            GUI.batch.Draw(GUI.crosshair, new Vector2(GUI.device.Viewport.Width / 2 - 5, GUI.device.Viewport.Height / 2 - 5), Color.White);
            GUI.batch.End();

            //Draw anything we want printed
            GUI.batch.Begin();
            for (int i = 0; i < GUI.strings.Count; i++)
            {
                GUI.batch.DrawString(GUI.font, (String)GUI.strings[i], new Vector2(100, 20 * i), Color.Black);
            }
            GUI.batch.DrawString(GUI.font, GUI.gameTime.ToString(), new Vector2(GUI.device.Viewport.Width - 50, 20), Color.Black);
            GUI.batch.End();
        }

        public static void drawInventoryHotBar(Inventory inventory)
        {
            itemRec.Y = device.Viewport.Height - 65;
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            batch.Draw(hotbar, hotBarRec, Color.White);

            for (int i = 0; i < 10; i++)
            {
                if (inventory.items[i] != null)
                {
                    itemRec.Width = 62;
                    itemRec.Height = 62;
                    itemRec.X = device.Viewport.Width / 2 + i * 66 - 328;
                    batch.Draw(Block.textureList[inventory.items[i].type], itemRec, Color.White);

                    batch.DrawString(
                        font,
                        inventory.items[i].amount.ToString(),
                        new Vector2(itemRec.X, itemRec.Y),
                        Color.Black);
                }
            }

            batch.End();
        }
    }
}
