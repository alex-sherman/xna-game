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
        public Mapgen.Heightmap heightMap;
        public World _world;
        public int size;
        public int landType;
        public Landchunk(World world, Vector3 location, int _size)
        {
            _world = world;
            this.location = location;
            this.size = _size/8;
            this.landType = Mapgen.Mapgen.rand.Next(20);
            this.generator = new Mapgen.Mapgen(world, size, location,landType);
            generator.generateLand();
            generator.generateMountain(size / 30, 20);
        }
        public Landchunk(World world, Vector3 location, float[] _heightMap)
        {
            _world = world;
            this.location = location;
            this.size = 599;
            this.landType = Mapgen.Mapgen.rand.Next(20);
            this.generator = new Mapgen.Mapgen(world, size, location, landType);
            heightMap = new Mapgen.Heightmap(_heightMap);
        }
        public override void Draw()
        {
            heightMap.Draw();
        }
        
    }
}
