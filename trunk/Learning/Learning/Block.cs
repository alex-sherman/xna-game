using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Learning
{
    class Block
    {
        static VertexDeclaration vertexDeclaration;
        static VertexPositionColor[] vertices;
        static VertexBuffer vertexBuffer;
        static short[] indices;
        static IndexBuffer indexBuffer;
        private Vector3 position;
        static public Effect effect;
        static float aspectRatio = 0.0f;
        static private ArrayList BlockList = new ArrayList();
        static Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        const int number_of_vertices = 24;
        const int number_of_indices = 36;


        public Block(Vector3 position)
        {
            this.position = position;
        }

        public static void addBlock(Vector3 position)
        {
            Block poo = new Block(position);
            BlockList.Add(poo);
        }

        public static void Draw(GraphicsDevice device,Matrix partialWorld,Vector3 cameraPos){
            foreach (Block block in Block.BlockList)
            {
                if (Block.aspectRatio == 0.0f)
                {
                    Block.aspectRatio = device.Viewport.AspectRatio;
                }
                Block.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(block.position-cameraPos)*partialWorld);
                foreach (EffectPass pass in Block.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    device.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.TriangleList,
                        vertices,
                        0,   // vertex buffer offset to add to each element of the index buffer
                        24,  // number of vertices to draw
                        indices,
                        0,   // first index element to read
                        12   // number of primitives to draw
                    );
                }
            }
        }
        
        public static void InitializeCube(GraphicsDeviceManager graphics)
        {
            
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );



            vertices = new VertexPositionColor[24];

            Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 bottomLeftFront = new Vector3(-1.0f, -1.0f, 1.0f);
            Vector3 topRightFront = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 topRightBack = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 bottomRightBack = new Vector3(1.0f, -1.0f, -1.0f);

            vertices[0] = new VertexPositionColor(topLeftFront, Color.Brown);
            vertices[1] = new VertexPositionColor(bottomLeftFront, Color.Brown);
            vertices[2] = new VertexPositionColor(topRightFront, Color.Brown);
            vertices[3] = new VertexPositionColor(bottomRightFront, Color.Brown);

            vertices[4] = new VertexPositionColor(topLeftBack, Color.Green);
            vertices[5] = new VertexPositionColor(topRightBack, Color.Green);
            vertices[6] = new VertexPositionColor(bottomLeftBack, Color.Green);
            vertices[7] = new VertexPositionColor(bottomRightBack, Color.Green);

            vertices[8] = new VertexPositionColor(topLeftFront, Color.Green);
            vertices[9] = new VertexPositionColor(topRightBack, Color.Green);
            vertices[10] = new VertexPositionColor(topLeftBack, Color.Green);
            vertices[11] = new VertexPositionColor(topRightFront, Color.Green);

            vertices[12] = new VertexPositionColor(bottomLeftFront, Color.Green);
            vertices[13] = new VertexPositionColor(bottomLeftBack, Color.Green);
            vertices[14] = new VertexPositionColor(bottomRightBack, Color.Green);
            vertices[15] = new VertexPositionColor(bottomRightFront, Color.Green);

            vertices[16] = new VertexPositionColor(topLeftFront, Color.Green);
            vertices[17] = new VertexPositionColor(bottomLeftBack, Color.Green);
            vertices[18] = new VertexPositionColor(bottomLeftFront, Color.Green);
            vertices[19] = new VertexPositionColor(topLeftBack, Color.Green);

            vertices[20] = new VertexPositionColor(topRightFront, Color.Green);
            vertices[21] = new VertexPositionColor(bottomRightFront, Color.Green);
            vertices[22] = new VertexPositionColor(bottomRightBack, Color.Green);
            vertices[23] = new VertexPositionColor(topRightBack, Color.Green);

            indices = new short[] {  0,  1,  2,  // red front face
                                     1,  3,  2,
                                     4,  5,  6,  // orange back face
                                     6,  5,  7,
                                     8,  9, 10,  // yellow top face
                                     8, 11,  9,
                                    12, 13, 14,  // purple bottom face
                                    12, 14, 15,
                                    16, 17, 18,  // blue left face
                                    19, 17, 16,
                                    20, 21, 22,  // green right face
                                    23, 20, 22  };

            vertexBuffer = new VertexBuffer(
                graphics.GraphicsDevice,
                vertexDeclaration,
                number_of_vertices,
                BufferUsage.None
                );


            vertexBuffer.SetData<VertexPositionColor>(vertices);

            indexBuffer = new IndexBuffer(graphics.GraphicsDevice,
                IndexElementSize.SixteenBits,
                number_of_indices,
                BufferUsage.None
                );

            indexBuffer.SetData<short>(indices);

        }
    }
}
