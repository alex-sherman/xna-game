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
        public static Game game;
        public static SpriteBatch batch;
        private static ArrayList strings = new ArrayList();
        private static Texture2D crosshair;
        public static Texture2D hotbar;
        public static Texture2D inventory;
        private static int lineNumber = 0;
        public static void Init(Game game)
        {
            GUI.game = game;
            GUI.font = game.Content.Load<SpriteFont>("GUIfont");
            GUI.crosshair = game.Content.Load<Texture2D>("Textures\\Crosshair");
            GUI.hotbar = game.Content.Load<Texture2D>("Textures\\Hotbar");
            GUI.inventory = game.Content.Load<Texture2D>("Textures\\Inventory");
            GUI.device = World.device;
            GUI.batch = new SpriteBatch(device);
        }
        public static void print(String toPrint)
        {
            if (GUI.strings.Count > 10) { GUI.strings.RemoveAt(0); }
            GUI.lineNumber++;
            GUI.strings.Add(GUI.lineNumber.ToString()+": "+toPrint);
        }
        public static void Draw(Inventory inventory)
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
            GUI.batch.End();


            inventory.Draw();
            
            
        }
    }
}
