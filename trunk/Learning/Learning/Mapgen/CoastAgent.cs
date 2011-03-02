﻿using System;
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
        public static int variance = 3;
        public static int minHeight = 0;
        public static int radius = 7;
        public static int maxTokens = 1000;
        public static int nToChange = 6;
        private Vector2 attractor;
        private Vector2 repulsor;
        int height = Mapgen.rand.Next(variance) + coastHeight;
        private Mapgen man;
        private int n = 0;
        private Vector2 direction = new Vector2(Mapgen.rand.Next(-1, 2), Mapgen.rand.Next(-1, 2));
        public CoastAgent(int tokens, Mapgen man){
            Vector2? poo = man.getRandomCoast();
            if (poo != null) { location = (Vector2)poo; }
            else { location = man.getRandomXY(); }
            this.tokens = tokens;
            this.man = man;
            attractor = man.getRandomXY();
            repulsor = man.getRandomXY();
            for (int i = 0; i < tokens; i++)
            {

            }
            while (this.tokens > maxTokens)
            {
                man.currentAgents.Add(new CoastAgent(man.getCoastInArea(location,10), this.tokens / 2, man));
                this.tokens /= 2;
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
            while (this.tokens > maxTokens)
            {
                man.currentAgents.Add(new CoastAgent(man.getCoastInArea(location, 10), this.tokens / 2, man));
                this.tokens /= 2;
            }
        }
        public bool step()
        {
            if (tokens <= 0) { return false; }
            List<Vector2> possiblePoints = man.getAdjacentNotCoast(location);
            direction = new Vector2(Mapgen.rand.Next(-1, 2), Mapgen.rand.Next(-1, 2));
            if (possiblePoints.Count == 0) {
                    getNewSpot();
                    return true;
            }
            float highestScore = score(possiblePoints[0]);
            Vector2 highestPoint = possiblePoints[0];
            float lowestScore = score(possiblePoints[0]);
            Vector2 lowestPoint = possiblePoints[0];
            foreach (Vector2 point in possiblePoints)
            {
                if (score(point) > highestScore)
                {
                    highestScore = score(point);
                    highestPoint = point;
                }
            }
            tokens-= radius*radius;
            highestPoint = (highestPoint - location) * radius + location;
            location = highestPoint;
            for (int x = (int)highestPoint.X-radius; x <= (int)highestPoint.X + radius; x++)
            {
                for (int y = (int)highestPoint.Y-radius; y <= (int)highestPoint.Y + radius; y++)
                {
                    if (x >= 0 && x < man.size && y >= 0 && y < man.size)
                    {
                        Vector2 curPoint = new Vector2(x, y);
                        if ((curPoint-location).Length() <= radius)
                        {
                            if (man.isCoast(x, y))
                            {

                                man.coastLine.Add(curPoint);
                            }
                            man.landHeight[x, y] = height;
                        }
                    }
                }
            }
            for (int x = (int)location.X - radius-1; x <= (int)location.X + radius+1; x++)
            {
                for (int y = (int)location.Y - radius - 1; y <= (int)location.Y + radius + 1; y++)
                {
                    if (x >= 0 && x < man.size && y >= 0 && y < man.size)
                    {
                        if (man.landHeight[x, y] >= coastHeight-2)
                        {
                            man.smooth(ref man.landHeight, x, y);
                        }
                        else
                        {
                            man.smooth(ref man.landHeight, x, y, (int)((location - new Vector2(x, y)).Length() / radius * 3));
                        }
                    }
                }
            }
            getNewSpot();
            n++;
            return true;

        }
        private void getNewSpot()
        {
            List<Vector2> possible = man.getCoastInArea(location,radius);
            if(possible.Count==0){
                Vector2? poo = man.getRandomCoast();
                if (poo != null) { location = (Vector2)poo; }
                else { location = man.getRandomXY(); }
            }
            else{ location = possible[Mapgen.rand.Next(possible.Count)];}
            
        }
        private float score(Vector2 point)
        {
            return (point - repulsor).LengthSquared() - (point - attractor).LengthSquared() + 10 * man.getEdgeDistance(point);
        }
    }
}
