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
        static VertexBuffer frontBuffer;
        static VertexBuffer leftBuffer;
        static VertexBuffer rightBuffer;
        static VertexBuffer backBuffer;
        static VertexBuffer topBuffer;
        static VertexBuffer bottomBuffer;
        static IndexBuffer indexBuffer;
        private static GraphicsDevice device;
        private static Effect effect;
        public static void InitializeCube(GraphicsDevice device, Effect effect)
        {
            Cube.device = device;
            Cube.effect = effect;


            float size = Cube.cubeSize;
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
            VertexPositionNormalTexture[] frontFace = new VertexPositionNormalTexture[]
            {
                // Front Surface
                new VertexPositionNormalTexture(bottomLeftFront,frontNormal,frontBottomLeft),
                new VertexPositionNormalTexture(topLeftFront ,frontNormal,frontTopLeft), 
                new VertexPositionNormalTexture(bottomRightFront,frontNormal,frontBottomRight),
                new VertexPositionNormalTexture(topRightFront,frontNormal,frontTopRight) 
            };
            VertexPositionNormalTexture[] backFace = new VertexPositionNormalTexture[]
            {
                // Back Surface
                new VertexPositionNormalTexture(bottomRightBack,backNormal,backBottomLeft),
                new VertexPositionNormalTexture(topRightBack,backNormal,backTopLeft), 
                new VertexPositionNormalTexture(bottomLeftBack,backNormal,backBottomRight),
                new VertexPositionNormalTexture(topLeftBack,backNormal,backTopRight)
            };
            VertexPositionNormalTexture[] leftFace = new VertexPositionNormalTexture[]
            {
                // Left Surface
                new VertexPositionNormalTexture(bottomLeftBack,leftNormal,leftBottomRight),
                new VertexPositionNormalTexture(topLeftBack,leftNormal,leftTopRight),
                new VertexPositionNormalTexture(bottomLeftFront,leftNormal,leftBottomLeft),
                new VertexPositionNormalTexture(topLeftFront,leftNormal,leftTopLeft)
            };
            VertexPositionNormalTexture[] rightFace = new VertexPositionNormalTexture[]
            {
                // Right Surface
                new VertexPositionNormalTexture(bottomRightFront,rightNormal,rightBottomLeft),
                new VertexPositionNormalTexture(topRightFront,rightNormal,rightTopLeft),
                new VertexPositionNormalTexture(bottomRightBack,rightNormal,rightBottomRight),
                new VertexPositionNormalTexture(topRightBack,rightNormal,rightTopRight)
            };
            VertexPositionNormalTexture[] topFace = new VertexPositionNormalTexture[]
            {
                // Top Surface
                new VertexPositionNormalTexture(topLeftFront,topNormal,TtopLeftFront),
                new VertexPositionNormalTexture(topLeftBack,topNormal,TtopLeftBack),
                new VertexPositionNormalTexture(topRightFront,topNormal,TtopRightFront),
                new VertexPositionNormalTexture(topRightBack,topNormal,TtopRightBack)
            };
            VertexPositionNormalTexture[] bottomFace = new VertexPositionNormalTexture[]
            {
                // Bottom Surface
                new VertexPositionNormalTexture(bottomLeftBack,bottomNormal,TbottomRightBack),
                new VertexPositionNormalTexture(bottomLeftFront,bottomNormal,TbottomRightFront),
                new VertexPositionNormalTexture(bottomRightBack,bottomNormal,TbottomLeftBack),
                new VertexPositionNormalTexture(bottomRightFront,bottomNormal,TbottomLeftFront)
            };
            short[] indices = new short[] { 
                0, 1, 2, 2, 1, 3
            };
            frontBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            frontBuffer.SetData<VertexPositionNormalTexture>(frontFace);

            backBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            backBuffer.SetData<VertexPositionNormalTexture>(backFace);

            leftBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            leftBuffer.SetData<VertexPositionNormalTexture>(leftFace);

            rightBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            rightBuffer.SetData<VertexPositionNormalTexture>(rightFace);

            topBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            topBuffer.SetData<VertexPositionNormalTexture>(topFace);

            bottomBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            bottomBuffer.SetData<VertexPositionNormalTexture>(bottomFace);

            indexBuffer = new IndexBuffer(Cube.device, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);

            indexBuffer.SetData<short>(indices);

        }
        public static void Draw(Vector3 position, World world, Model myModel){
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
        public static void Draw(Vector3 position, World world, Texture2D texture)
        {
            device.BlendState = BlendState.AlphaBlend;
            if (texture == null) { return; }
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).position);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect);
        }
        public static void Draw(Vector3 position, World world, Texture2D texture, bool[] sides)
        {
            device.BlendState = BlendState.AlphaBlend;
            if (texture == null) { return; }
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).position);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect,sides);
        }
        public static void Draw(Effect effect)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Cube.device.Indices = indexBuffer;
                Cube.device.SetVertexBuffer(frontBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                Cube.device.SetVertexBuffer(leftBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                Cube.device.SetVertexBuffer(rightBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                Cube.device.SetVertexBuffer(topBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                Cube.device.SetVertexBuffer(bottomBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                Cube.device.SetVertexBuffer(backBuffer);
                Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
            }
        }
        public static void Draw(Effect effect,bool[] sides)
        {
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Cube.device.Indices = indexBuffer;
                if (sides[0])
                {
                    Cube.device.SetVertexBuffer(frontBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
                if (sides[2])
                {
                    Cube.device.SetVertexBuffer(leftBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
                if (sides[3])
                {
                    Cube.device.SetVertexBuffer(rightBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
                if (sides[4])
                {
                    Cube.device.SetVertexBuffer(topBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
                if (sides[5])
                {
                    Cube.device.SetVertexBuffer(bottomBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
                if (sides[1])
                {
                    Cube.device.SetVertexBuffer(backBuffer);
                    Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
                }
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
            Draw(Cube.effect);
            poo = new RasterizerState();
            poo.FillMode = FillMode.Solid;
            device.RasterizerState = poo;
        }

        public static void Draw(Vector3 position, World world, Texture2D texture, float scale, float rotation)
        {
            if (texture == null) { return; }
            Matrix worldMatrix = Matrix.CreateRotationY(rotation)* Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            Cube.effect.Parameters["WorldViewProj"].SetValue(worldMatrix * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(worldMatrix);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect);
        }
    }
}
