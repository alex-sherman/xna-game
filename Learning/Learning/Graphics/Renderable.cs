using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Learning
{
    interface Renderable
    {
        VertexBuffer getVbuffer();
        IndexBuffer getIbuffer();
        Matrix getWorld();
        void Draw();
        bool Visible(BoundingFrustum view);
    }
}
