using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    abstract class GenRend : Renderable
    {
        public Vector3 location = new Vector3(0,0,0);
        virtual public VertexBuffer getVbuffer() { return null; }
        virtual public IndexBuffer getIbuffer() { return null; }
        virtual public Matrix getWorld() { return Matrix.CreateTranslation(location); }
        virtual public void Draw()
        {
            GraphicsEngine.Render(this as Renderable);
        }
        virtual public bool Visible(BoundingFrustum view) { return true; }
    }
}
