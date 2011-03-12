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
        public VertexBuffer vBuffer = null;
        public IndexBuffer iBuffer = null;
        virtual public VertexBuffer getVbuffer() { return vBuffer; }
        virtual public IndexBuffer getIbuffer() { return iBuffer; }
        virtual public Matrix getWorld() { return Matrix.CreateTranslation(location); }
        virtual public void Draw()
        {
            GraphicsEngine.Render(this as Renderable);
        }
        virtual public bool Visible(BoundingFrustum view) { return true; }
    }
}
