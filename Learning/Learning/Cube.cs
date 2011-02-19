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
        public const float cubeSize = .5f;
        static VertexDeclaration vertexDeclaration;
        static VertexPositionColor[] vertices;
        static VertexBuffer vertexBuffer;
        static IndexBuffer indexBuffer;
        private static GraphicsDevice device;
        private static BasicEffect effect;
        public static void InitializeCube(GraphicsDevice device, BasicEffect effect)
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
            Vector3 topLeftFront = new Vector3(-size, size, size);
            Vector3 bottomLeftFront = new Vector3(-size, -size, size);
            Vector3 topRightFront = new Vector3(size, size, size);
            Vector3 bottomRightFront = new Vector3(size, -size, size);
            Vector3 topLeftBack = new Vector3(-size, size, -size);
            Vector3 topRightBack = new Vector3(size, size, -size);
            Vector3 bottomLeftBack = new Vector3(-size, -size, -size);
            Vector3 bottomRightBack = new Vector3(size, -size, -size);

            Vector2 sideTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 sideTopRight = new Vector2(.5f, 0.0f);
            Vector2 sideBottomLeft = new Vector2(0.0f, 1f);
            Vector2 sideBottomRight = new Vector2(0.5f, 1f);
            Vector2 TtopLeftBack = new Vector2(0.5f, 0.0f);
            Vector2 TtopRightBack = new Vector2(1.0f, 0.0f);
            Vector2 TtopLeftFront = new Vector2(0.5f, 1f);
            Vector2 TtopRightFront = new Vector2(1.0f, 1f);

            Vector3 frontNormal = new Vector3(0, 0, 1);
            Vector3 backNormal = new Vector3(0, 0, -1);
            Vector3 leftNormal = new Vector3(-1, 0, 0);
            Vector3 rightNormal = new Vector3(1, 0, 0);
            Vector3 topNormal = new Vector3(0, 1, 0);
            Vector3 bottomNormal = new Vector3(0, -1, 0);
            VertexPositionNormalTexture[] boxData = new VertexPositionNormalTexture[]
            {
                // Front Surface
                new VertexPositionNormalTexture(bottomLeftFront,frontNormal,sideBottomLeft),
                new VertexPositionNormalTexture(topLeftFront ,frontNormal,sideTopLeft), 
                new VertexPositionNormalTexture(bottomRightFront,frontNormal,sideBottomRight),
                new VertexPositionNormalTexture(topRightFront,frontNormal,sideTopRight),  

                // Back Surface
                new VertexPositionNormalTexture(bottomRightBack,backNormal,sideBottomLeft),
                new VertexPositionNormalTexture(topRightBack,backNormal,sideTopLeft), 
                new VertexPositionNormalTexture(bottomLeftBack,backNormal,sideBottomRight),
                new VertexPositionNormalTexture(topLeftBack,backNormal,sideTopRight), 

                // Left Surface
                new VertexPositionNormalTexture(bottomLeftBack,leftNormal,sideBottomRight),
                new VertexPositionNormalTexture(topLeftBack,leftNormal,sideTopRight),
                new VertexPositionNormalTexture(bottomLeftFront,leftNormal,sideBottomLeft),
                new VertexPositionNormalTexture(topLeftFront,leftNormal,sideTopLeft),
                // Right Surface
                new VertexPositionNormalTexture(bottomRightFront,rightNormal,sideBottomRight),
                new VertexPositionNormalTexture(topRightFront,rightNormal,sideTopRight),
                new VertexPositionNormalTexture(bottomRightBack,rightNormal,sideBottomLeft),
                new VertexPositionNormalTexture(topRightBack,rightNormal,sideTopLeft),
                // Top Surface
                new VertexPositionNormalTexture(topLeftFront,topNormal,TtopLeftFront),
                new VertexPositionNormalTexture(topLeftBack,topNormal,TtopLeftBack),
                new VertexPositionNormalTexture(topRightFront,topNormal,TtopRightFront),
                new VertexPositionNormalTexture(topRightBack,topNormal,TtopRightBack),

                // Bottom Surface
                new VertexPositionNormalTexture(bottomLeftBack,bottomNormal,TtopLeftBack),
                new VertexPositionNormalTexture(bottomLeftFront,bottomNormal,TtopLeftFront),
                new VertexPositionNormalTexture(bottomRightBack,bottomNormal,TtopRightBack),
                new VertexPositionNormalTexture(bottomRightFront,bottomNormal,TtopRightFront),
            };
            short[] indices = new short[] { 
                0, 1, 2, 2, 1, 3,   
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 10, 9, 11, 
                12, 13, 14, 14, 13, 15, 
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23
            };
            vertexBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Cube.device, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionNormalTexture>(boxData);
            indexBuffer.SetData<short>(indices);

        }

        public static void Draw(Vector3 position, World world, Texture2D texture)
        {
            if (texture == null) { return; }
            Matrix partialWorld = world.partialWorld;
            Cube.effect.World = Matrix.CreateTranslation(position);
            Cube.effect.View = partialWorld;
            Cube.effect.Texture = texture;
            foreach(EffectPass pass in effect.CurrentTechnique.Passes){
                pass.Apply();
            effect.CurrentTechnique.Passes[0].Apply();
            Cube.device.SetVertexBuffer(vertexBuffer);
            Cube.device.Indices = indexBuffer;
            Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
            }
        }
        public static void Draw(Vector3 position, World world, Texture2D texture, float scale, float rotation)
        {
            if (texture == null) { return; }
            Matrix partialWorld = world.partialWorld;
            Cube.effect.World = Matrix.CreateScale(scale)*Matrix.CreateRotationY(rotation)*Matrix.CreateTranslation(position);
            Cube.effect.View = partialWorld;
            Cube.effect.Texture = texture;
            effect.CurrentTechnique.Passes[0].Apply();
            Cube.device.SetVertexBuffer(vertexBuffer);
            Cube.device.Indices = indexBuffer;
            Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
        }
    }
}
