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
        private World _world;
        public int[,] rockHeight;
        public int[,] landHeight;
        public int[,] lastRockHeight;
        public int[,] lastLandHeight;
        public List<MapgenAgent> currentAgents = new List<MapgenAgent>();
        public List<Vector2> coastLine = new List<Vector2>();
        public OctreeNode renderTree;
        public int size;
        public static Random rand = new Random();
        public List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
        public List<int> indices = new List<int>();
        public VertexBuffer vBuffer;
        public IndexBuffer iBuffer;
        public Mapgen(World world)
        {
            _world = world;
            this.size = 250;
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
        public void setBuffers()
        {
            vBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, vertices.Count, BufferUsage.WriteOnly);
            vBuffer.SetData(vertices.ToArray());
            iBuffer = new IndexBuffer(Cube.device, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
            iBuffer.SetData(indices.ToArray());
        }
        public void getVertices()
        {
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    int j = landHeight[i,k];
                    if (i<size-1 && k < size-1)
                    {
                        int o = vertices.Count();
                            indices.AddRange(getIndices(i,k,o));
                            vertices.AddRange(getVertices(i, k));
                            
                    }
                }
            }
        }

        public List<VertexPositionNormalTexture> getVertices(int x, int y)
        {
            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            float currentHeight = 0;
            Vector2 currentPoint = new Vector2(x, y);
            Vector2 heightPoint = new Vector2(x, y);
            for (float i = 0; i <= 1; i += .25f)
            {
                for (float k = 0; k <= 1; k += .25f)
                {
                    currentPoint = new Vector2(i+x, k+y);
                    for (int io = 0; io <= 1; io++)
                    {
                        for (int ko = 0; ko <= 1; ko++)
                        {
                            heightPoint = new Vector2(io, ko)+currentPoint;
                            if ((heightPoint - currentPoint).LengthSquared() == 0)
                            {
                                currentHeight = landHeight[(int)heightPoint.X, (int)heightPoint.Y];
                                io = 2;
                                ko = 2;
                                break;
                            }
                            else
                            {
                                currentHeight += landHeight[x + io, y + ko]*((heightPoint-currentPoint).Length());
                            }
                        }
                    }
                    vertices.Add(new VertexPositionNormalTexture(new Vector3(currentPoint.X, currentHeight, currentPoint.Y), Vector3.Zero, new Vector2(i, k)));
                        
                    }
            }/*
            vertices.Add(new VertexPositionNormalTexture(new Vector3(x, landHeight[x, x], y), Vector3.Zero,  new Vector2(0,0)));
            vertices.Add(new VertexPositionNormalTexture(new Vector3(x, landHeight[x, x+1], y+1), Vector3.Zero,  new Vector2(0,1)));
            vertices.Add(new VertexPositionNormalTexture(new Vector3(x + 1, landHeight[x + 1, y+1], y+1), Vector3.Zero, new Vector2(1,1)));
            vertices.Add(new VertexPositionNormalTexture(new Vector3(x + 1, landHeight[x+1, y], y), Vector3.Zero,  new Vector2(1,0)));
            */
            
            return vertices;
        }
        public int[] getIndices(int x, int y, int o)
        {
            return new int[] {
                                o,o+6,o+1,
                                o,o+6,o+5,
                                o+1,o+7,o+2,
                                o+1,o+7,o+6,
                                o+2,o+8,o+3,
                                o+2,o+8,o+7,
                                o+3,o+9,o+4,
                                o+3,o+9,o+8,
                                o+5,o+11,o+6,
                                o+5,o+11,o+10,
                                o+6,o+12,o+7,
                                o+6,o+12,o+11,
                                o+7,o+13,o+8,
                                o+7,o+13,o+12,
                                o+8,o+14,o+9,
                                o+8,o+14,o+13,
                                o+10,o+16,o+11,
                                o+10,o+16,o+15,
                                o+11,o+17,o+12,
                                o+11,o+17,o+16,
                                o+12,o+18,o+13,
                                o+12,o+18,o+17,
                                o+13,o+19,o+14,
                                o+13,o+19,o+18,
                                o+15,o+21,o+16,
                                o+15,o+21,o+20,
                                o+16,o+22,o+17,
                                o+16,o+22,o+21,
                                o+17,o+23,o+18,
                                o+17,o+23,o+22,
                                o+18,o+24,o+19,
                                o+18,o+24,o+23,

                            };
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
        public Vector2? getRandomCoast()
        {
            if (coastLine.Count == 0) { return null; }
            return coastLine[Mapgen.rand.Next(coastLine.Count)];
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
            return landHeight[x,y]>=CoastAgent.coastHeight-2;
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
            currentAgents = new List<MapgenAgent>();
            currentAgents.Add(new CoastAgent(tokens, this));
            bool alive = true;
            while (alive)
            {
                alive = false;
                findLand();
                foreach (MapgenAgent agent in currentAgents)
                {
                    if (agent.step()) { alive = true; }
                }
            }
        }
        public void step()
        {
            foreach (MapgenAgent agent in currentAgents)
            {
                agent.step();
            }

        }
        
        public void smoothLand(int number, int tokens)
        {
            findLand();
            currentAgents = new List<MapgenAgent>();
            for (int i = 0; i < number; i++)
            {
                currentAgents.Add(new SmoothingAgent(getRandomXY(), tokens, this));
            }
            bool alive = true;
            while (alive)
            {
                alive = false;
                findLand();
                foreach (MapgenAgent agent in currentAgents)
                {
                    if (agent.step()) { alive = true; }
                }
            }
        }
        public void smoothCoast(int number, int tokens)
        {
            findCurrentCoastline();
            currentAgents = new List<MapgenAgent>();
            for (int i = 0; i < number; i++)
            {
                currentAgents.Add(new SmoothingAgent(getRandomXY(), tokens, this));
            }
            bool alive = true;
            while (alive)
            {
                alive = false;
                findCurrentCoastline();
                foreach (MapgenAgent agent in currentAgents)
                {
                    if (agent.step()) { alive = true; }
                }
            }
        }
        public void generateRock(int agents, int tokens )
        {
            currentAgents = new List<MapgenAgent>();
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
        }
        public void smooth(ref int[,] heightMap, int x, int y)
        {
            smooth(ref heightMap, x, y, 0);
        }
        public void smooth(ref int[,] heightMap, int x, int y,int variance)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
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
                heightMap[x, y] = total / 9 + rand.Next(-1, 2) * rand.Next(variance);
            }
        }
        public Vector2 getRandomXY()
        {
            return new Vector2(rand.Next(size), rand.Next(size));
        }
    }
}
