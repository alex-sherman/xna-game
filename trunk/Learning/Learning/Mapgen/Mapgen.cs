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
        public float[,] landHeight;
        public float[,] highResLandHeight;
        public List<MapgenAgent> currentAgents = new List<MapgenAgent>();
        public List<Vector2> coastLine = new List<Vector2>();
        public int size;
        public static Random rand = new Random();
        public List<MultiTex> vertices = new List<MultiTex>();
        public List<int> indices = new List<int>();
        public VertexBuffer vBuffer;
        public IndexBuffer iBuffer;
        public static Texture2D grass;
        public static Texture2D sand;
        public struct MultiTex : IVertexType
        {
            public Vector3 Position;
            public Vector2 TextureCoordinate;
            public Vector3 Normal;
            public Vector4 BlendWeight;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1)
        );
        public MultiTex(Vector3 pos, Vector3 normal, Vector2 texPos, Vector4 blendWeights)
        {
            Position = pos;
            TextureCoordinate = texPos;
            Normal = normal;
            BlendWeight = blendWeights;
        }

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
        };
        public Mapgen(World world,int size)
        {
            _world = world;
            this.size = size;
            rockHeight = new int[size, size];
            landHeight = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    rockHeight[i, j] = RockAgent.maxHeight - RockAgent.variance;
                    landHeight[i, j] = CoastAgent.minHeight;
                }
            }
        }
        public void setBuffers()
        {
            
            vBuffer = new VertexBuffer(GraphicsEngine.device, MultiTex.VertexDeclaration, vertices.Count, BufferUsage.WriteOnly);
            vBuffer.SetData(vertices.ToArray());
            iBuffer = new IndexBuffer(GraphicsEngine.device, IndexElementSize.ThirtyTwoBits, indices.Count, BufferUsage.WriteOnly);
            iBuffer.SetData(indices.ToArray());
        }
        public void getVertices()
        {
            highResLandHeight = new float[size * 10, size * 10];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for(int x = i*10;x<i*10+10;x++){
                        for (int y = j*10; y<j*10 + 10; y++)
                        {
                            highResLandHeight[x, y] = landHeight[i, j];
                        }
                    }
                }
            }
            smoothMap(ref highResLandHeight, size * 10,25);
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (i<size-1 && k < size-1)
                    {
                        int o = vertices.Count();
                            indices.AddRange(getIndices(i,k,o));
                            vertices.AddRange(getVertices(i*4, k*4));
                    }
                }
            }
        }

        public List<MultiTex> getVertices(int x, int y)
        {
            List<MultiTex> vertices = new List<MultiTex>();
            Vector2 currentPoint = new Vector2(x, y);
            Vector2 heightPoint = new Vector2(x, y);
            for (int i = 0; i <= 4; i += 1)
            {
                
                
                for (int k = 0; k <= 4; k += 1)
                {
                    float height = highResLandHeight[x+i, y+k];
                    MultiTex vertex = new MultiTex(new Vector3((x+i)/10.0f, highResLandHeight[x+i, y+k], (y+k)/10.0f), Vector3.Zero, new Vector2(((i+x))/10f, ((k+y))/10f), new Vector4(0, 0, 0, 0));
                    vertex.BlendWeight.X = MathHelper.Clamp(1.0f - Math.Abs(height) / 5.0f, 0, 1);
                    vertex.BlendWeight.Y = MathHelper.Clamp(1.0f - Math.Abs(height - 9) / 5.0f, 0, 1);
                    vertex.BlendWeight.Z = MathHelper.Clamp(1.0f - Math.Abs(height - 17) / 10.0f, 0, 1);
                    vertex.BlendWeight.W = MathHelper.Clamp(1.0f - Math.Abs(height - 30) / 6.0f, 0, 1);
                    if (height > 8) { 
                    }
                    vertices.Add(vertex);
                        
                }
            }
            
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
        public Vector2 getRandomCoast()
        {
            if (coastLine.Count == 0) { return getRandomXY(); }
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
            return (float)Math.Min(Math.Min(Math.Pow(size-point.X, 2), Math.Pow(size-point.Y, 2)), Math.Min(Math.Pow(point.X, 2), Math.Pow(point.Y, 2)));
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
        public void generateMountain(int number, int tokens)
        {
            findLand();
            currentAgents = new List<MapgenAgent>();
            for (int i = 0; i < number; i++)
            {
                currentAgents.Add(new RockAgent(getRandomXY(), tokens, this));
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
        public void smooth(ref float[,] heightMap, int x, int y, int arraySize)
        {
            smooth(ref heightMap, x, y, 0, arraySize);
        }
        public void smooth(ref float[,] heightMap, int x, int y,int variance,int arraySize)
        {
            
                if (x >= 0 && x < arraySize && y >= 0 && y < arraySize)
                {
                    int curX;
                    int curY;
                    float total = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            curX = x + i;
                            curY = y + j;
                            if (curX >= 0 && curX < arraySize && curY >= 0 && curY < arraySize)
                            {
                                total += heightMap[curX, curY];
                            }
                            else { total += heightMap[x, y]; }
                        }
                    }
                    heightMap[x, y] = (total / 9 + heightMap[x, y]) / 2 + rand.Next(-1, 2) * rand.Next(variance);
                }
            
        }
        public void smoothMap(ref float[,] heightMap,int arraySize, int repitions)
        {
            for (int r = 0; r < repitions; r++)
            {
                for (int x = 0; x < arraySize; x++)
                {
                    for (int y = 0; y < arraySize; y++)
                    {
                        smooth(ref heightMap, x, y, 0, arraySize);
                    }
                }
            }
        }

        public void smoothMap(ref float[,] heightMap, int arraySize)
        {
            for (int x = 0; x < arraySize; x++)
            {
                for (int y = 0; y < arraySize; y++)
                {
                    smooth(ref heightMap, x, y, 0, arraySize);
                }
            }
        }
        public Vector2 getRandomXY()
        {
            return new Vector2(rand.Next(size), rand.Next(size));
        }
    }
}
