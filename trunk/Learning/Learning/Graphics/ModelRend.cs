using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    abstract class ModelRend : GenRend
    {
        public Model model;
        override public void Draw()
        {
            GraphicsEngine.Render(this as Renderable);
        }
        public ModelRend(Model model)
        {
            this.model = model;
            this.location = new Vector3(0, 0, 0);
        }
        public override Matrix getWorld()
        {
            return Matrix.CreateTranslation(location);
        }
    }
}
