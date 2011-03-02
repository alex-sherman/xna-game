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
        public static VertexBuffer stripVBuffer;
        public static DynamicVertexBuffer instanceVertexBuffer;
        public static VertexDeclaration instanceVertexDeclaration;
        static IndexBuffer indexBuffer;
        public static short[] indices;
        public static VertexPositionNormalTexture[][] faces;
        public static Vector3 topLeftFront = new Vector3(-cubeSize, cubeSize, cubeSize);
        public static Vector3 bottomLeftFront = new Vector3(-cubeSize, -cubeSize, cubeSize);
        public static Vector3 topRightFront = new Vector3(cubeSize, cubeSize, cubeSize);
        public static Vector3 bottomRightFront = new Vector3(cubeSize, -cubeSize, cubeSize);
        public static Vector3 topLeftBack = new Vector3(-cubeSize, cubeSize, -cubeSize);
        public static Vector3 topRightBack = new Vector3(cubeSize, cubeSize, -cubeSize);
        public static Vector3 bottomLeftBack = new Vector3(-cubeSize, -cubeSize, -cubeSize);
        public static Vector3 bottomRightBack = new Vector3(cubeSize, -cubeSize, -cubeSize);

        //Texture Positions
        public static Vector2 TtopLeftBack = new Vector2(0f, 0.0f);
        public static Vector2 TtopRightBack = new Vector2(.25f, 0.0f);
        public static Vector2 TtopLeftFront = new Vector2(0f, .25f);
        public static Vector2 TtopRightFront = new Vector2(.25f, .25f);

        public static Vector2 TbottomLeftBack = new Vector2(0f, .5f);
        public static Vector2 TbottomLeftFront = new Vector2(0f, .75f);
        public static Vector2 TbottomRightBack = new Vector2(.25f, .5f);
        public static Vector2 TbottomRightFront = new Vector2(0.25f, .75f);

        public static Vector2 frontTopLeft = new Vector2(0.0f, 0.25f);
        public static Vector2 frontTopRight = new Vector2(.25f, 0.25f);
        public static Vector2 frontBottomLeft = new Vector2(0.0f, .5f);
        public static Vector2 frontBottomRight = new Vector2(0.25f, .5f);

        public static Vector2 rightTopLeft = new Vector2(0.25f, 0.25f);
        public static Vector2 rightTopRight = new Vector2(.5f, 0.25f);
        public static Vector2 rightBottomLeft = new Vector2(0.25f, .5f);
        public static Vector2 rightBottomRight = new Vector2(0.5f, .5f);

        public static Vector2 backTopLeft = new Vector2(0.5f, 0.25f);
        public static Vector2 backTopRight = new Vector2(.75f, 0.25f);
        public static Vector2 backBottomLeft = new Vector2(0.5f, .5f);
        public static Vector2 backBottomRight = new Vector2(0.75f, .5f);


        public static Vector2 leftTopLeft = new Vector2(0.75f, 0.25f);
        public static Vector2 leftTopRight = new Vector2(1f, 0.25f);
        public static Vector2 leftBottomLeft = new Vector2(0.75f, .5f);
        public static Vector2 leftBottomRight = new Vector2(1f, .5f);

        public static Vector3 frontNormal = new Vector3(0, 0, 1);
        public static Vector3 backNormal = new Vector3(0, 0, -1);
        public static Vector3 leftNormal = new Vector3(-1, 0, 0);
        public static Vector3 rightNormal = new Vector3(1, 0, 0);
        public static Vector3 topNormal = new Vector3(0, 1, 0);
        public static Vector3 bottomNormal = new Vector3(0, -1, 0);

        public static GraphicsDevice device;
        private static Effect effect;
        public static void InitializeCube(GraphicsDevice device, Effect effect)
        {
            Cube.device = device;
            Cube.effect = effect;
            Graphics.GraphicsEngine.effect = effect;
            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                }
            );


            vertices = new VertexPositionColor[24];
            
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
            VertexPositionNormalTexture[] stripCube = new VertexPositionNormalTexture[] {
                // Top Surface
                new VertexPositionNormalTexture(topLeftBack,topNormal,TtopLeftBack),
                new VertexPositionNormalTexture(topRightBack,topNormal,TtopRightBack),
                new VertexPositionNormalTexture(topRightFront,topNormal,TtopRightFront),
                new VertexPositionNormalTexture(topLeftFront,topNormal,TtopLeftFront),
                // Front Surface
                new VertexPositionNormalTexture(bottomLeftFront,frontNormal,frontBottomLeft),
                new VertexPositionNormalTexture(topRightFront,frontNormal,frontTopRight),
                //Right face
                new VertexPositionNormalTexture(bottomRightFront,rightNormal,rightBottomLeft),
                new VertexPositionNormalTexture(topRightFront,rightNormal,rightTopLeft),
                //Back face
                new VertexPositionNormalTexture(bottomLeftBack,backNormal,backBottomRight),
                new VertexPositionNormalTexture(topLeftBack,backNormal,backTopRight),
                //Left Face
                new VertexPositionNormalTexture(topLeftFront,leftNormal,leftTopLeft),
                new VertexPositionNormalTexture(bottomLeftFront,leftNormal,leftBottomLeft),
                //Bottom face
                new VertexPositionNormalTexture(bottomRightBack,bottomNormal,TbottomLeftBack),
                new VertexPositionNormalTexture(bottomRightFront,bottomNormal,TbottomLeftFront)

            };
            stripVBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 14, BufferUsage.WriteOnly);
            stripVBuffer.SetData(stripCube);
            faces = new VertexPositionNormalTexture[][] { leftFace, rightFace, topFace, bottomFace, frontFace, backFace };

            indices = new short[] { 
                0, 1, 2, 2, 1, 3,   
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 10, 9, 11, 
                12, 13, 14, 14, 13, 15, 
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23
            };
            instanceVertexDeclaration = new VertexDeclaration
            (
            new VertexElement(0,  VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
            );
            vertexBuffer = new VertexBuffer(Cube.device, VertexPositionNormalTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(Cube.device, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);
            VertexPositionNormalTexture[] boxData = new VertexPositionNormalTexture[24];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    boxData[i * 4 + j] = faces[i][j];
                }
            }
            vertexBuffer.SetData<VertexPositionNormalTexture>(boxData);
            indexBuffer.SetData<short>(indices);
            

        }
        public static VertexPositionNormalTexture[] getFace(int index,Vector3 position)
        {
            VertexPositionNormalTexture[] face = new VertexPositionNormalTexture[4];
            for (int i = 0; i < 4; i++)
            {
                face[i] = new VertexPositionNormalTexture();
                face[i].Position = faces[index][i].Position + position;
                face[i].Normal = faces[index][i].Normal;
                face[i].TextureCoordinate = faces[index][i].TextureCoordinate;
            }
            return face;
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
        public static void Draw(Matrix[] instances, World world, Texture2D texture)
        {
            device.BlendState = BlendState.Opaque;
            if (texture == null) { return; }
            Cube.effect.Parameters["view"].SetValue(world.partialWorld);
            Cube.effect.Parameters["proj"].SetValue(world.projection);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect, instances);
        }
        public static void Draw(Vector3 position, World world, Texture2D texture)
        {
            device.BlendState = BlendState.Opaque;
            if (texture == null) { return; }
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).Position);
            Cube.effect.Parameters["UserTexture"].SetValue(texture);
            Draw(Cube.effect);
        }
        public static void Draw(Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Texture"];
            effect.CurrentTechnique.Passes[0].Apply();
            Cube.device.SetVertexBuffers(vertexBuffer);
            Cube.device.Indices = indexBuffer;
            Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
        }

        public static void Draw(Effect effect, Matrix[] instances)
        {
            if (instances.Length != 0)
            {
                instanceVertexBuffer = new DynamicVertexBuffer(Cube.device, instanceVertexDeclaration, instances.Length, BufferUsage.WriteOnly);
                instanceVertexBuffer.SetData(instances, 0, instances.Length);
                effect.CurrentTechnique = effect.Techniques["InstanceTexture"];
                Cube.device.SetVertexBuffers(new VertexBufferBinding(vertexBuffer, 0, 0), new VertexBufferBinding(instanceVertexBuffer, 0, 1));
                Cube.device.Indices = indexBuffer;
                effect.CurrentTechnique.Passes[0].Apply();
                Cube.device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12, instances.Length);
            }
        }
        public static DynamicVertexBuffer getInstanceBuffer(Matrix[] instances)
        {
            instanceVertexBuffer = new DynamicVertexBuffer(Cube.device, instanceVertexDeclaration, instances.Length, BufferUsage.WriteOnly);
            instanceVertexBuffer.SetData(instances, 0, instances.Length);
            return instanceVertexBuffer;
        }
        public static void Draw(Vector3 position, World world, bool wireframe, float scale)
        {
            RasterizerState poo = new RasterizerState();
            poo.FillMode = FillMode.WireFrame;
            device.RasterizerState = poo;
            Cube.effect.Parameters["WorldViewProj"].SetValue(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position) * world.partialWorld * world.projection);
            Cube.effect.Parameters["world"].SetValue(Matrix.CreateTranslation(position) * Matrix.CreateScale(scale));
            Cube.effect.Parameters["cameraPosition"].SetValue(((Player)world.players[0]).Position);
            Draw(Cube.effect);
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
            Draw(Cube.effect);
        }
    }
}