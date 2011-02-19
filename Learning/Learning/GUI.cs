using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private static int lineNumber = 0;
        public static void Init(SpriteFont font,GraphicsDevice device,Texture2D crosshair,Texture2D hotbar)
        {
            GUI.hotbar = hotbar;
            GUI.crosshair = crosshair;
            GUI.device = device;
            GUI.font = font;
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
            GUI.batch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            GUI.batch.Draw(GUI.crosshair, new Vector2(GUI.device.Viewport.Width / 2-5, GUI.device.Viewport.Height / 2-5), Color.White);
            
            GUI.batch.End();
            GUI.batch.Begin();
            for (int i = 0; i < GUI.strings.Count; i++)
            {
                GUI.batch.DrawString(GUI.font, (String)GUI.strings[i], new Vector2(100, 20 * i), Color.Black);
            }
            Rectangle curItemRec = new Rectangle();
            curItemRec.Height = 64;
            curItemRec.Width = 65;
            curItemRec.Y = GUI.device.Viewport.Height - 66;
            GUI.batch.Draw(GUI.hotbar, new Vector2(GUI.device.Viewport.Width / 2 - 331, GUI.device.Viewport.Height - 68), Color.White);
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if(inventory.items[i]!=null){
                    curItemRec.X = GUI.device.Viewport.Width/2 + i * 66 - 329;
                    GUI.batch.Draw(Block.textureList[inventory.items[i].type], curItemRec, Color.White);
                    GUI.batch.DrawString(GUI.font, inventory.count[i].ToString(), new Vector2(GUI.device.Viewport.Width / 2 + i * 66 - 329, GUI.device.Viewport.Height - 64), Color.Pink);
                }
            }
            GUI.batch.End();
        }
    }
}
