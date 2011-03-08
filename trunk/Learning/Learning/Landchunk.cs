using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class Landchunk : GenRend
    {
        public Mapgen.Mapgen generator;
        public float[,] heightMap;
        public World _world;
        public int size;
        VertexBuffer vBuffer;
        IndexBuffer iBuffer;
        public Landchunk(World world, Vector3 location, int _size)
        {
            _world = world;
            this.location = location;
            this.size = _size/4;
            this.generator = new Mapgen.Mapgen(world, size, location);
            generator.generateLand(size*size*6/10);
            generator.generateMountain(size / 50, 20);
            heightMap = generator.landHeight;
        }
        public void resetBuffers()
        {
            generator.getVertices();
            generator.setBuffers(out vBuffer, out iBuffer);
        }
        override public VertexBuffer getVbuffer() { return vBuffer; }
        override public IndexBuffer getIbuffer() { return iBuffer; }
        //override public Matrix getWorld() { return Matrix.CreateTranslation(location); }
        
    }
}
