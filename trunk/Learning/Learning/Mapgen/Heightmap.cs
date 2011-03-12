using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Mapgen
{
    class Heightmap : GenRend
    {
        public float[,] heights;
        public List<MultiTex> vertices;
        public List<int> indices;
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
        public Heightmap(float[] heightlist)
        {
            heights = new float[600, 600];
            for (int x = 0; x < 600; x++)
            {
                for (int y = 0; y < 600; y++)
                {
                    heights[x, y] = heightlist[x + y * 600]*100;
                }
            }
            getVertices();
            setBuffers();

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
            int size = (heights.GetUpperBound(0) + 1);
            vertices = new List<MultiTex>();
            for (int i = 0; i < size; i += 1)
            {
                for (int k = 0; k < size; k += 1)
                {
                    vertices.AddRange(getVertices(i, k));
                }
            }

            indices = new List<int>(getIndices(0, size));
        }

        public List<MultiTex> getVertices(int x, int y)
        {
            int a = 8;
            int size = heights.GetUpperBound(0);
            float b = .5f;
            int x1 = x;
            int y1 = y;
            if (x1 >= size) { x1 = size - 1; }
            if (y1 >= size) { y1 = size - 1; }
            List<MultiTex> vertices = new List<MultiTex>();
            Vector2 currentPoint = new Vector2(x, y);
            Vector2 heightPoint = new Vector2(x, y);
            float height = heights[x1, y1];
            MultiTex vertex = new MultiTex(new Vector3((x * a), height * 3.2f, (y * a)), Vector3.Zero, new Vector2(((x * b)) / 10f, ((y * b)) / 10f), new Vector4(0, 0, 0, 0));
            vertex.BlendWeight.X = MathHelper.Clamp(1.0f - Math.Abs(height) / 10.0f, 0, 1);
            vertex.BlendWeight.Y = MathHelper.Clamp(1.0f - Math.Abs(height - 20) / 15.0f, 0, 1);
            vertex.BlendWeight.Z = MathHelper.Clamp(1.0f - Math.Abs(height - 50) / 20.0f, 0, 1);
            //vertex.BlendWeight.W = MathHelper.Clamp(1.0f - Math.Abs(height - 30) / 6.0f, 0, 1);
            vertices.Add(vertex);
            return vertices;
        }
        public int[] getIndices(int o, int squareSize)
        {
            int[] toReturn = new int[(squareSize - 1) * (squareSize - 1) * 6];
            for (int i = 0; i < toReturn.Length / 6; i++)
            {
                toReturn[i * 6] = i + ((i) / (squareSize - 1)) + o;
                toReturn[i * 6 + 1] = toReturn[i * 6] + squareSize + 1;
                toReturn[i * 6 + 2] = toReturn[i * 6] + 1;
                toReturn[i * 6 + 3] = toReturn[i * 6];
                toReturn[i * 6 + 4] = toReturn[i * 6] + squareSize + 1;
                toReturn[i * 6 + 5] = toReturn[i * 6] + squareSize;
            }

            return toReturn;
        }
    }
}
