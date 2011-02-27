using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Mapgen
{
    class Mapgen
    {
        public int[,] rockHeight;
        public int[,] landHeight;
        public int[,] lastRockHeight;
        public int[,] lastLandHeight;
        public List<MapgenAgent> currentAgents = new List<MapgenAgent>();
        public List<Vector2> coastLine = new List<Vector2>();
        public OctreeNode node;
        public int size;
        public static Random rand = new Random();
        public List<VertexPositionNormalTexture> vertices;
        public Mapgen(OctreeNode node)
        {
            this.node = node;
            this.size = (int)node.nodeSize*2;
            rockHeight = new int[size, size];
            landHeight = new int[size, size];
            lastLandHeight = new int[size, size];
            lastRockHeight = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    rockHeight[i, j] = RockAgent.maxHeight - RockAgent.variance;
                    landHeight[i, j] = CoastAgent.minHeight;
                    lastRockHeight[i, j] = RockAgent.maxHeight - RockAgent.variance;
                    lastLandHeight[i, j] = CoastAgent.minHeight;
                }
            }
        }
        public void findCurrentCoastline()
        {
            coastLine = new List<Vector2>();
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (isCoast(i,k)) { coastLine.Add(new Vector2(i, k)); }
                }
            }
        }
        public void findLand()
        {
            coastLine = new List<Vector2>();
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (landHeight[i,k]>=CoastAgent.coastHeight) { coastLine.Add(new Vector2(i, k)); }
                }
            }
        }
        public void step()
        {
            findLand();
             foreach (MapgenAgent agent in currentAgents)
            {
                agent.step();
            }
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (landHeight[i, k] != lastLandHeight[i, k])
                    {
                        node.addBlock(i - size / 2, landHeight[i, k], k - size / 2, 1);
                        lastLandHeight[i, k] = landHeight[i, k];
                    }
                }
            }
        }
        public Vector2? getRandomCoast()
        {
            if (coastLine.Count == 0) { return null; }
            return coastLine[Mapgen.rand.Next(coastLine.Count)];
        }
        public void generateVertices()
        {
            vertices = new List<VertexPositionNormalTexture>();
            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    vertices.Add(new VertexPositionNormalTexture(new Vector3(x, rockHeight[x, z], z), Vector3.Zero, new Vector2(0, 0)));
                    vertices.Add(new VertexPositionNormalTexture(new Vector3(x+1, rockHeight[x, z], z), Vector3.Zero, new Vector2(0, 0)));
                    vertices.Add(new VertexPositionNormalTexture(new Vector3(x, rockHeight[x, z], z+1), Vector3.Zero, new Vector2(0, 0)));
                }
            }
        }
        public bool isCoast(int x, int y)
        {
            List<Vector2> toReturn = new List<Vector2>();
            if (landHeight[x, y] >= CoastAgent.coastHeight) { return false; }
            bool land = false;
            bool notLand = false;
            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int curX = i + x;
                    int curY = k + y;
                    if (curX > 0 && curX < size && curY > 0 && curY < size)
                    {
                        if (landHeight[curX, curY] < CoastAgent.coastHeight)
                        {
                            notLand = true;
                        }
                        else
                        {
                            land = true;
                        }
                    }
                }
            }
            return land && notLand;
        }
        public float getEdgeDistance(Vector2 point)
        {
            return (float)Math.Min(Math.Min(Math.Pow(point.X, 2) - Math.Pow(size, 2), Math.Pow(point.Y, 2) - Math.Pow(size, 2)), Math.Min(Math.Pow(point.X, 2), Math.Pow(point.Y, 2)));
        }
        public List<Vector2> getCoastInArea(Vector2 point, int radius)
        {
            List<Vector2> toReturn = new List<Vector2>();
            for (int i = -radius; i <= radius; i++)
            {
                for (int k = -radius; k <= radius; k++)
                {
                    int curX = (int)point.X + i;
                    int curY = (int)point.Y + k;
                    if (curY>=0 && curY<size && curX>=0 && curX<size && isCoast(curX, curY))
                    {
                        toReturn.Add(new Vector2(curX, curY));
                    }
                }
            }
            return toReturn;
        }
        public bool isLand(int x, int y)
        {
            return landHeight[x,y]>=CoastAgent.coastHeight;
        }
        public List<Vector2> getAdjacentNotCoast(Vector2 point)
        {
            List<Vector2> toReturn = new List<Vector2>();
            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int curX = i + (int)point.X;
                    int curY = k + (int)point.Y;
                    if (curX > 0 && curX < size && curY > 0 && curY < size && landHeight[curX, curY] < CoastAgent.coastHeight)
                    {
                        toReturn.Add(new Vector2(curX, curY));
                    }
                }
            }
            return toReturn;
        }
        public void generateLand(int tokens)
        {
            currentAgents.Add(new CoastAgent(tokens, this));
            bool alive = true;
            /*while (alive)
            {
                alive = false;
                //findCurrentCoastline();
                findLand();
                foreach (MapgenAgent agent in currentAgents)
                {
                    if (agent.step()) { alive = true; }
                }
            }
            currentAgents = new List<MapgenAgent>();
            for (int i = 0; i < node.nodeSize * 2; i++)
            {
                for (int k = 0; k < node.nodeSize * 2; k++)
                {
                    node.addVisibleBlock(i - size / 2, landHeight[i,k], k - size / 2, 1);
                    for (int j = 0; j <= landHeight[i,k]; j++)
                    {
                        //if (j == landHeight[i, k])
                        //{
                        //    node.addVisibleBlock(i - size / 2, j, k - size / 2, 1);
                        //}
                        //else
                        //{
                            node.addBlock(i - size / 2, j, k - size / 2, 1);
                        //}
                    }
                }
            }*/
        }
        public void generateRock(int agents, int tokens)
        {
            for (int i = 0; i < agents; i++)
            {
                currentAgents.Add(new RockAgent(getRandomXY(), tokens,this));
            }
            bool alive = true;
            while (alive)
            {
                alive = false;
                foreach (MapgenAgent agent in currentAgents)
                {
                    if (agent.step()) { alive = true; }
                }
            }
            currentAgents = new List<MapgenAgent>();
            for (int i = 0; i < node.nodeSize*2; i++)
            {
                for (int k = 0; k < node.nodeSize * 2; k++)
                {
                    for (int j = RockAgent.minHeight; j <= rockHeight[i,k]; j++)
                    {
                        //if (j == rockHeight[i, k])
                        //{
                        //    node.addVisibleBlock(i - size / 2, j, k - size / 2, 1);
                        //}
                        //else
                        //{
                            node.addBlock(i - size / 2, j, k - size / 2, 1);
                        //}
                    }
                }
            }
        }
        public void smooth(ref int[,] heightMap, int x, int y)
        {
            int[] toAdd = { -2, -1, 1, 2 };
            int curX;
            int curY;
            int total = heightMap[x, y];
            foreach (int add in toAdd)
            {
                curX = x + add;
                curY = y + add;
                if (curX >= 0 && curX < size && curY >= 0 && curY < size)
                {
                    total += heightMap[curX, curY];
                }
                else { total += heightMap[x, y]; }
                curY = y - add;
                if (curX >= 0 && curX < size && curY >= 0 && curY < size)
                {
                    total += heightMap[curX, curY];
                }
                else { total += heightMap[x, y]; }

            }
            heightMap[x, y] = total / 9;
        }
        public Vector2 getRandomXY()
        {
            return new Vector2(rand.Next(size), rand.Next(size));
        }
    }
}
