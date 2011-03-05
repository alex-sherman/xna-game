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
        public float[,] landHeight;
        public float[,] highResLandHeight;
        public static Random rand = new Random();
        public List<MapgenAgent> currentAgents = new List<MapgenAgent>();
        public List<Vector2> coastLine = new List<Vector2>();
        public int size;
        public List<MultiTex> vertices = new List<MultiTex>();
        public List<int> indices = new List<int>();
        public VertexBuffer vBuffer;
        public IndexBuffer iBuffer;
        public Vector3 location;
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
        public Mapgen(World world,int size,Vector3 location)
        {
            _world = world;
            this.size = size;
            this.location = location;
            landHeight = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
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
        public float[,] getHeightmap()
        {
            return landHeight;
        }
        public void getVertices()
        {
            highResLandHeight = new float[size * 2, size * 2];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for(int x = i*2;x<i*2;x++){
                        for (int y = j * 2; y < j * 2 ; y++)
                        {
                            highResLandHeight[x, y] = landHeight[i, j];
                        }
                    }
                }
            }
            //smoothMap(ref highResLandHeight, size * 10,15);
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    vertices.AddRange(getVertices(i, k ));
                    
                }
            }
            indices.AddRange(getIndices(0, size));
        }

        public List<MultiTex> getVertices(int x, int y)
        {
            int a = 20;
            int b = 10;
            List<MultiTex> vertices = new List<MultiTex>();
            Vector2 currentPoint = new Vector2(x, y);
            Vector2 heightPoint = new Vector2(x, y);
            float height = landHeight[x, y];
            MultiTex vertex = new MultiTex(new Vector3((x * a), landHeight[x, y]*1.1f, (y * a)), Vector3.Zero, new Vector2(((x * b)) / 10f, ((y * b)) / 10f), new Vector4(0, 0, 0, 0));
            vertex.BlendWeight.X = MathHelper.Clamp(1.0f - Math.Abs(height) / 10.0f, 0, 1);
            vertex.BlendWeight.Y = MathHelper.Clamp(1.0f - Math.Abs(height - 20) / 15.0f, 0, 1);
            vertex.BlendWeight.Z = MathHelper.Clamp(1.0f - Math.Abs(height - 50) / 20.0f, 0, 1);
            //vertex.BlendWeight.W = MathHelper.Clamp(1.0f - Math.Abs(height - 30) / 6.0f, 0, 1);
            vertices.Add(vertex);
            return vertices;
        }
        public int[] getIndices(int o,int squareSize)
        {
            int[] toReturn = new int[(squareSize-1) * (squareSize-1) * 6];
            for (int i = 0; i < toReturn.Length/6; i++)
            {
                toReturn[i * 6] = i + ((i) / (squareSize-1)) + o;
                toReturn[i * 6 + 1] = toReturn[i * 6] + squareSize + 1;
                toReturn[i * 6 + 2] = toReturn[i * 6] + 1;
                toReturn[i * 6 + 3] = toReturn[i * 6];
                toReturn[i * 6 + 4] = toReturn[i * 6] + squareSize + 1;
                toReturn[i * 6 + 5] = toReturn[i * 6] + squareSize;
            }

            return toReturn;
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
