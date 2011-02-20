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
        private static SpriteFont font;
        private static GraphicsDevice device;
        private static SpriteBatch batch;
        private static ArrayList strings = new ArrayList();
        private static Texture2D crosshair;
        private static Texture2D hotbar;
        private static Texture2D inventory;
        private static int lineNumber = 0;
        public static void Init(Game game)
        {

            GUI.font = game.Content.Load<SpriteFont>("GUIfont");
            GUI.crosshair = game.Content.Load<Texture2D>("Textures\\Crosshair");
            GUI.hotbar = game.Content.Load<Texture2D>("Textures\\Hotbar");
            GUI.inventory = game.Content.Load<Texture2D>("Textures\\Inventory");
            GUI.device = World.device;
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
            GUI.batch.End();

            //Draw anything we want printed
            GUI.batch.Begin();
            for (int i = 0; i < GUI.strings.Count; i++)
            {
                GUI.batch.DrawString(GUI.font, (String)GUI.strings[i], new Vector2(100, 20 * i), Color.Black);
            }
            //Draw the hotbar
            Rectangle curItemRec = new Rectangle();
            curItemRec.Height = 64;
            curItemRec.Width = 65;
            curItemRec.Y = GUI.device.Viewport.Height - 66;
            GUI.batch.Draw(GUI.hotbar, new Vector2(GUI.device.Viewport.Width / 2 - 331, GUI.device.Viewport.Height - 68), Color.White);
            if (inventory.inventoryUp)
            {
                GUI.batch.Draw(GUI.crosshair, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
                GUI.batch.Draw(GUI.inventory, new Vector2(GUI.device.Viewport.Width / 2 - 331, GUI.device.Viewport.Height - 331), Color.White);
            }
            else
            {
                GUI.batch.Draw(GUI.crosshair, new Vector2(GUI.device.Viewport.Width / 2 - 5, GUI.device.Viewport.Height / 2 - 5), Color.White);
                //Draw any items in it
                for (int i = 0; i < inventory.items.Length; i++)
                {
                    if (inventory.items[i] != null)
                    {
                        curItemRec.X = GUI.device.Viewport.Width / 2 + i * 66 - 329;
                        GUI.batch.Draw(Block.textureList[inventory.items[i].type], curItemRec, Color.White);
                        GUI.batch.DrawString(GUI.font, inventory.items[i].amount.ToString(), new Vector2(GUI.device.Viewport.Width / 2 + i * 66 - 329, GUI.device.Viewport.Height - 64), Color.Black);
                    }
                }
            }
            GUI.batch.End();
        }
    }
}
