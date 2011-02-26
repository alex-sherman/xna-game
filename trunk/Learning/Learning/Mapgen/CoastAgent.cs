using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Mapgen
{
    class CoastAgent : MapgenAgent
    {
        public Vector2 location;
        public int tokens = 0;
        public static int coastHeight = 10;
        public static int variance = 5;
        public static int minHeight = 0;
        public static int radius = 10;
        public static int maxTokens = 5;
        public static int nToChange = 6;
        private Vector2 attractor;
        private Vector2 repulsor;
        private Mapgen man;
        private int n = 0;
        private Vector2 direction = new Vector2(Mapgen.rand.Next(-1, 2), Mapgen.rand.Next(-1, 2));
        public CoastAgent(int tokens, Mapgen man){
            Vector2? poo = man.getRandomCoast();
            if (poo != null) { location = (Vector2)poo; }
            else { location = man.getRandomXY(); }
            this.tokens = tokens;
            this.man = man;
            while (tokens > maxTokens)
            {
                man.currentAgents.Add(new CoastAgent(man.getCoastInArea(location,10), tokens / 2, man));
                tokens /= 2;
            }
        }
        public CoastAgent(List<Vector2> possible, int tokens, Mapgen man)
        {
            if(possible.Count==0){ location = man.getRandomXY();}
            else{ location = possible[Mapgen.rand.Next(possible.Count)];}
            this.tokens = tokens;
            this.man = man;
            attractor = man.getRandomXY();
            repulsor = man.getRandomXY();
        }
        public bool step()
        {
            if (tokens == 0) { return false; }
            List<Vector2> possiblePoints = man.getAdjacentNotCoast(location);
            if (possiblePoints.Count == 0) { getNewSpot(); return true; }
            float highestScore = score(possiblePoints[0]);
            Vector2 currentPoint = possiblePoints[0];
            foreach (Vector2 point in possiblePoints)
            {
                if (score(point) > highestScore)
                {
                    highestScore = score(point);
                    currentPoint = point;
                }
            }
            tokens--;
            man.coastLine.Add(currentPoint);
            man.landHeight[(int)currentPoint.X, (int)currentPoint.Y] = CoastAgent.coastHeight;
            getNewSpot();
            return true;

        }
        private void getNewSpot()
        {
            Vector2? poo = man.getRandomCoast();
            if (poo != null) { location = (Vector2)poo; }
            else { location = man.getRandomXY(); }
        }
        private float score(Vector2 point)
        {
            return (point - repulsor).LengthSquared() - (point - attractor).LengthSquared() + 3 * man.getEdgeDistance(point);
        }
    }
}
