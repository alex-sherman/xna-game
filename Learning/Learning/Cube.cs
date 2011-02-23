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
        public static short[] indices;
        public static short[] front;
        public static short[] back;
        public static short[] left;
        public static short[] right;
        public static short[] top;
        public static short[] bottom;

        private static GraphicsDevice device;
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
            Vector3 topLeftFront = new Vector3(-size, size, size);
            Vector3 bottomLeftFront = new Vector3(-size, -size, size);
            Vector3 topRightFront = new Vector3(size, size, size);
            Vector3 bottomRightFront = new Vector3(size, -size, size);
            Vector3 topLeftBack = new Vector3(-size, size, -size);
            Vector3 topRightBack = new Vector3(size, size, -size);
            Vector3 bottomLeftBack = new Vector3(-size, -size, -size);
            Vector3 bottomRightBack = new Vector3(size, -size, -size);

            //Texture Positions
            Vector2 TtopLeftBack = new Vector2(0f, 0.0f);
            Vector2 TtopRightBack = new Vector2(.25f, 0.0f);
            Vector2 TtopLeftFront = new Vector2(0f, .25f);
            Vector2 TtopRightFront = new Vector2(.25f, .25f);

            Vector2 TbottomLeftBack = new Vector2(0f, .5f);
            Vector2 TbottomLeftFront = new Vector2(0f, .75f);
            Vector2 TbottomRightBack = new Vector2(.25f, .5f);
            Vector2 TbottomRightFront = new Vector2(0.25f, .75f);

            Vector2 frontTopLeft = new Vector2(0.0f, 0.25f);
            Vector2 frontTopRight = new Vector2(.25f, 0.25f);
            Vector2 frontBottomLeft = new Vector2(0.0f, .5f);
            Vector2 frontBottomRight = new Vector2(0.25f, .5f);

            Vector2 rightTopLeft = new Vector2(0.25f, 0.25f);
            Vector2 rightTopRight = new Vector2(.5f, 0.25f);
            Vector2 rightBottomLeft = new Vector2(0.25f, .5f);
            Vector2 rightBottomRight = new Vector2(0.5f, .5f);

            Vector2 backTopLeft = new Vector2(0.5f, 0.25f);
            Vector2 backTopRight = new Vector2(.75f, 0.25f);
            Vector2 backBottomLeft = new Vector2(0.5f, .5f);
            Vector2 backBottomRight = new Vector2(0.75f, .5f);


            Vector2 leftTopLeft = new Vector2(0.75f, 0.25f);
            Vector2 leftTopRight = new Vector2(1f, 0.25f);
            Vector2 leftBottomLeft = new Vector2(0.75f, .5f);
            Vector2 leftBottomRight = new Vector2(1f, .5f);

            Vector3 frontNormal = new Vector3(0, 0, 1);
            Vector3 backNormal = new Vector3(0, 0, -1);
            Vector3 leftNormal = new Vector3(-1, 0, 0);
            Vector3 rightNormal = new Vector3(1, 0, 0);
            Vector3 topNormal = new Vector3(0, 1, 0);
            Vector3 bottomNormal = new Vector3(0, -1, 0);
            VertexPositionNormalTexture[] boxData = new VertexPositionNormalTexture[]
            {
                // Front Surface
                new VertexPositionNormalTexture(bottomLeftFront,frontNormal,frontBottomLeft),
                new VertexPositionNormalTexture(topLeftFront ,frontNormal,frontTopLeft), 
                new VertexPositionNormalTexture(bottomRightFront,frontNormal,frontBottomRight),
                new VertexPositionNormalTexture(topRightFront,frontNormal,frontTopRight),  

                // Back Surface
                new VertexPositionNormalTexture(bottomRightBack,backNormal,backBottomLeft),
                new VertexPositionNormalTexture(topRightBack,backNormal,backTopLeft), 
                new VertexPositionNormalTexture(bottomLeftBack,backNormal,backBottomRight),
                new VertexPositionNormalTexture(topLeftBack,backNormal,backTopRight), 

                // Left Surface
                new VertexPositionNormalTexture(bottomLeftBack,leftNormal,leftBottomRight),
                new VertexPositionNormalTexture(topLeftBack,leftNormal,leftTopRight),
                new VertexPositionNormalTexture(bottomLeftFront,leftNormal,leftBottomLeft),
                new VertexPositionNormalTexture(topLeftFront,leftNormal,leftTopLeft),
                // Right Surface
                new VertexPositionNormalTexture(bottomRightFront,rightNormal,rightBottomLeft),
                new VertexPositionNormalTexture(topRightFront,rightNormal,rightTopLeft),
                new VertexPositionNormalTexture(bottomRightBack,rightNormal,rightBottomRight),
                new VertexPositionNormalTexture(topRightBack,rightNormal,rightTopRight),
                // Top Surface
                new VertexPositionNormalTexture(topLeftFront,topNormal,TtopLeftFront),
                new VertexPositionNormalTexture(topLeftBack,topNormal,TtopLeftBack),
                new VertexPositionNormalTexture(topRightFront,topNormal,TtopRightFront),
                new VertexPositionNormalTexture(topRightBack,topNormal,TtopRightBack),

                // Bottom Surface
                new VertexPositionNormalTexture(bottomLeftBack,bottomNormal,TbottomRightBack),
                new VertexPositionNormalTexture(bottomLeftFront,bottomNormal,TbottomRightFront),
                new VertexPositionNormalTexture(bottomRightBack,bottomNormal,TbottomLeftBack),
                new VertexPositionNormalTexture(bottomRightFront,bottomNormal,TbottomLeftFront),
            };
            indices = new short[] { 
                0, 1, 2, 2, 1, 3,   
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 10, 9, 11, 
                12, 13, 14, 14, 13, 15, 
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23
            };
            front = new short[] { 
                0, 1, 2, 2, 1, 3 
            };
            back = new short[] { 
                4, 5, 6, 6, 5, 7
            };
            left = new short[] { 
                8, 9, 10, 10, 9, 11
            };
            right = new short[] { 
                12, 13, 14, 14, 13, 15
            };
            top = new short[] { 
                16, 17, 18, 18, 17, 19
            };
            bottom = new short[] { 
                20, 21, 22, 22, 21, 23
            };
            vertexBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Cube.device, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionNormalTexture>(boxData);
            indexBuffer.SetData<short>(indices);

        }
        public static void Draw(Vector3 position, World world, Model myModel)
        {
            foreach (ModelMesh mesh in myModel.Meshes)
            {


                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateTranslation(position);
                    effect.View = world.partialWorld;
                    effect.Projection = world.projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
        public static void Draw(Vector3 position, World world, Texture2D texture,IndexBuffer indexBuffer)
        {
            device.BlendState = BlendState.AlphaBlend;
            if (texture == null) { return; }
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).position);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect, indexBuffer);
        }
        public static void Draw(Effect effect,IndexBuffer indexBuffer)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Cube.device.SetVertexBuffer(vertexBuffer);
                Cube.device.Indices = indexBuffer;
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
            }
        }

        public static void Draw(Vector3 position, World world, bool wireframe, float scale)
        {
            RasterizerState poo = new RasterizerState();
            poo.FillMode = FillMode.WireFrame;
            device.RasterizerState = poo;
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position) * Matrix.CreateScale(scale));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).position);
            Draw(Cube.effect,Cube.indexBuffer);
            poo = new RasterizerState();
            poo.FillMode = FillMode.Solid;
            device.RasterizerState = poo;
        }

        public static void Draw(Vector3 position, World world, Texture2D texture, float scale, float rotation)
        {
            if (texture == null) { return; }
            Matrix worldMatrix = Matrix.CreateRotationY(rotation) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            Cube.effect.Parameters["WorldViewProj"].SetValue(worldMatrix * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(worldMatrix);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect,Cube.indexBuffer);
        }
    }
}