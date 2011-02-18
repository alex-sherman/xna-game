using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Learning
{
    class Cube
    {

        const int number_of_vertices = 24;
        const int number_of_indices = 36;
        public const float cubeSize = 1.0f;
        static VertexDeclaration vertexDeclaration;
        static VertexPositionColor[] vertices;
        static VertexBuffer vertexBuffer;
        static IndexBuffer indexBuffer;
        private static GraphicsDevice device;
        public static Vector3 topLeftFront;
        private static Effect effect;

        public static void InitializeCube(GraphicsDevice device, Effect effect)
        {
            Cube.device = device;
            Cube.effect = effect;
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );


            float size = Cube.cubeSize;
            vertices = new VertexPositionColor[24];
            topLeftFront = new Vector3(-size, size, size);
            Vector3 bottomLeftFront = new Vector3(-size, -size, size);
            Vector3 topRightFront = new Vector3(size, size, size);
            Vector3 bottomRightFront = new Vector3(size, -size, size);
            Vector3 topLeftBack = new Vector3(-size, size, -size);
            Vector3 topRightBack = new Vector3(size, size, -size);
            Vector3 bottomLeftBack = new Vector3(-size, -size, -size);
            Vector3 bottomRightBack = new Vector3(size, -size, -size);


            Vector2 topLeft = new Vector2(0.0f, 0.0f);
            Vector2 topCenter = new Vector2(0.5f, 0.0f);
            Vector2 topRight = new Vector2(1.0f, 0.0f);
            Vector2 bottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 bottomRight = new Vector2(1.0f, 1.0f);
            VertexPositionTexture[] boxData = new VertexPositionTexture[]
            {
                // Front Surface
                new VertexPositionTexture(bottomLeftFront,bottomLeft),
                new VertexPositionTexture(topLeftFront ,topLeft), 
                new VertexPositionTexture(bottomRightFront,bottomRight),
                new VertexPositionTexture(topRightFront,topRight),  

                // Front Surface
                new VertexPositionTexture(bottomRightBack,bottomLeft),
                new VertexPositionTexture(topRightBack,topLeft), 
                new VertexPositionTexture(bottomLeftBack,bottomRight),
                new VertexPositionTexture(topLeftBack,topRight), 

                // Left Surface
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),topLeft),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),bottomRight),
                new VertexPositionTexture(topLeftFront,topRight),

                // Right Surface
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, -1.0f),topRight),

                // Top Surface
                new VertexPositionTexture(topLeftFront,bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, -1.0f),topRight),

                // Bottom Surface
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),topRight),
            };
            short[] indices = new short[] { 
                0, 1, 2, 2, 1, 3,   
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 10, 9, 11, 
                12, 13, 14, 14, 13, 15, 
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23
            };
            vertexBuffer = new VertexBuffer(Cube.device, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Cube.device, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionTexture>(boxData);
            indexBuffer.SetData<short>(indices);

        }

        public static void Draw(Vector3 position, World world, Texture2D texture)
        {
            Matrix partialWorld = world.partialWorld;
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(position) * partialWorld);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            effect.CurrentTechnique.Passes[0].Apply();
            Cube.device.SetVertexBuffer(vertexBuffer);
            Cube.device.Indices = indexBuffer;
            Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
        }
    }
}
