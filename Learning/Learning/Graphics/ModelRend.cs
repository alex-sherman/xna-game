using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Learning
{
    abstract class ModelRend : GenRend, Renderable
    {
        public Model model;
        override public void Draw()
        {
            GraphicsEngine.Draw(this);
        }
        public override Matrix getWorld()
        {
            return Matrix.CreateTranslation(location);
        }
    }
}
