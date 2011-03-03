using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Mapgen
{
    class SmoothingAgent : MapgenAgent
    {
        public Vector2 location;
        public int tokens = 0;
        public static int maxHeight = -10;
        public static int variance = 5;
        public static int minHeight = -20;
        public static int radius = 2;
        public static int nToChange = 6;
        private int n = 0;
        private Mapgen man;
        private Vector2 direction = new Vector2(Mapgen.rand.Next(-1,2), Mapgen.rand.Next(-1,2));
        public SmoothingAgent(Vector2 start, int tokens, Mapgen man)
        {
            this.location = start;
            this.tokens = tokens;
            this.man = man;
        }
        public bool step()
        {
            if (this.tokens <= 0) { return false; }

            if ((int)location.X + radius >= man.size || (int)location.X - radius < 0 || 
                (int)location.Y + radius >= man.size || (int)location.Y - radius < 0) {
                    if (man.getRandomCoast() != null)
                    {
                        this.location = (Vector2)man.getRandomCoast();
                    }
                    else { return false; }
                return true;
            }

            this.tokens--;
            int height = maxHeight - Mapgen.rand.Next(variance);
            for(int x = (int)location.X-radius;x<=(int)location.X+radius;x++){
                for (int y = (int)location.Y - radius; y <= (int)location.Y + radius; y++)
                {
                    man.smooth(ref man.landHeight, x, y,man.size);
                }
            }
            n++;
            if (n >= Mapgen.rand.Next(nToChange))
            {
                n = 0;
                direction = new Vector2(Mapgen.rand.Next(-1, 2), Mapgen.rand.Next(-1, 2));
            }
            List<Vector2> possible = man.getCoastInArea(location, 5 * radius);
            if (possible.Count != 0)
            {
                this.location = possible[Mapgen.rand.Next(possible.Count)];
            }
            else this.location = new Vector2(-1, -1);
            return true;
        }
    }
}
