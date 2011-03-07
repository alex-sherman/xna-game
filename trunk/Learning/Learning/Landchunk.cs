using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class Landchunk : Graphics.Renderable
    {
        public float[,] landHeight;
        public Mapgen.Mapgen generator;
        public Vector3 location;
        public World _world;
        public int size;
        public Graphics.Water water;
        VertexBuffer vBuffer;
        IndexBuffer iBuffer;
        public Landchunk(World world, Vector3 location, int size)
        {
            _world = world;
            this.location = location;
            this.size = size;
            this.generator = new Mapgen.Mapgen(world, size, location);
            water = new Graphics.Water(size, _world);
            generator.generateLand(size*size*6/10);
            generator.smoothMap(ref generator.landHeight, generator.size, 15);
            generator.generateMountain(size / 50, 20);
            generator.smoothMap(ref generator.landHeight, generator.size, 2);
            generator.getVertices();
            generator.setBuffers(out vBuffer, out iBuffer);
        }
        public VertexBuffer getVbuffer() { return vBuffer; }
        public IndexBuffer getIbuffer() { return iBuffer; }
        public Matrix getWorld() { return Matrix.CreateTranslation(location); } 
    }
}
