using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Mapgen
{
    class RockAgent : MapgenAgent
    {
        public Vector2 location;
        public int tokens = 0;
        public static int maxHeight = 25;
        public static int variance = 5;
        public static int minHeight = 15;
        public static int initialRadius = 4;
        private int radius = 5;
        public static int radiusVariance = 2;
        public static int nToChange = 6;
        private int n = 0;
        private Mapgen man;
        private Vector2 direction = new Vector2(Mapgen.rand.Next(-1,2), Mapgen.rand.Next(-1,2));
        public RockAgent(Vector2 start, int tokens, Mapgen man)
        {
            this.radius = initialRadius + Mapgen.rand.Next(radiusVariance) * Mapgen.rand.Next(-1, 2);
            this.location = start;
            this.tokens = tokens;
            this.man = man;
        }
        public bool step()
        {
            if (this.tokens == 0) { return false; }

            if ((int)location.X + radius*2 >= man.size || (int)location.X - radius*2 < 0 || 
                (int)location.Y + radius*2 >= man.size || (int)location.Y - radius*2 < 0) { 
                this.location = (Vector2)man.getRandomCoast();
                return true;
            }

            this.tokens--;
            int height = maxHeight - Mapgen.rand.Next(variance);
            for(int x = (int)location.X-radius;x<=(int)location.X+radius;x++){
                for (int y = (int)location.Y - radius; y <= (int)location.Y + radius; y++)
                {
                    Vector2 curPoint = new Vector2(x, y);
                    if ((curPoint - location).Length() <= radius)
                    {
                        if ((curPoint - location).Length() != 0)
                            man.landHeight[x, y] = (int)((height+radius / (curPoint - location).LengthSquared()));
                        else
                            man.landHeight[x, y] = height;
                        man.smooth(ref man.landHeight, x, y, 0);
                    }
                }
            }
            man.smooth(ref man.landHeight, (int)location.X, (int)location.Y, 1);
             
            n++;
            if (n >= Mapgen.rand.Next(nToChange))
            {
                n = 0;
                direction = new Vector2(Mapgen.rand.Next(-1, 2), Mapgen.rand.Next(-1, 2));
            }
            this.location += direction*radius;
            return true;
        }
    }
}
