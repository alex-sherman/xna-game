using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning.Graphics
{
    class GraphicsEngine
    {
        public static Effect effect;
        public static GraphicsDevice device;
        public static World world;
        public static Texture2D grass;
        public static Texture2D sand;
        public static Texture2D rock;
        public static void SetTextures(Texture2D grass, Texture2D sand, Texture2D rock)
        {
            GraphicsEngine.grass = grass;
            GraphicsEngine.sand = sand;
            GraphicsEngine.rock = rock;
        }
        public static void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            if (vertexBuffer != null && indexBuffer != null)
            {
                RasterizerState poo = new RasterizerState();
                //poo.FillMode = FillMode.WireFrame;
                //Cube.device.RasterizerState = poo;
                effect.CurrentTechnique = effect.Techniques["MultiTexture"];
                effect.Parameters["UserTextureA"].SetValue(sand);
                effect.Parameters["UserTextureB"].SetValue(grass);
                effect.Parameters["UserTextureC"].SetValue(rock);
                effect.Parameters["UserTextureD"].SetValue(grass);
                effect.Parameters["view"].SetValue(world.partialWorld);
                effect.Parameters["proj"].SetValue(world.projection);
                Cube.device.SetVertexBuffers(vertexBuffer);
                Cube.device.Indices = indexBuffer;
                effect.CurrentTechnique.Passes[0].Apply();

                for (int j = 0; j <= indexBuffer.IndexCount / 3000000; j += 1)
                {
                    if ((j + 1) * 3000000 >= indexBuffer.IndexCount)
                    {
                        Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, indexBuffer.IndexCount - j * 3000000, j * 3000000, (indexBuffer.IndexCount - j * 3000000) / 3);
                        break;
                    }
                    else
                    {
                        Cube.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, 3000000, j * 3000000, (3000000) / 3);

                    }
                }
            }
        }

        //Not currently used
        public static void Draw(DynamicVertexBuffer instanceVertexBuffer, VertexBuffer vBuffer, Texture2D texture)
        {
            if (instanceVertexBuffer!=null && instanceVertexBuffer.VertexCount>0)
            {
                
                effect.CurrentTechnique = effect.Techniques["InstanceTexture"];
                effect.Parameters["UserTextureA"].SetValue(texture);
                effect.Parameters["view"].SetValue(world.partialWorld);
                effect.Parameters["proj"].SetValue(world.projection);
                Cube.device.SetVertexBuffers(new VertexBufferBinding(vBuffer, 0, 0), new VertexBufferBinding(instanceVertexBuffer, 0, 1));
                effect.CurrentTechnique.Passes[0].Apply();
                Cube.device.DrawInstancedPrimitives(PrimitiveType.TriangleStrip, 0, 0, 14, 0, 12, instanceVertexBuffer.VertexCount);
            }
        }

        //Probably useless
        public static void Draw(VertexBuffer vertexBuffer, Texture2D texture)
        {
            if (vertexBuffer != null)
            {
                effect.CurrentTechnique = effect.Techniques["Texture"];
                effect.Parameters["UserTextureA"].SetValue(texture);
                effect.Parameters["view"].SetValue(world.partialWorld);
                effect.Parameters["proj"].SetValue(world.projection);
                Cube.device.SetVertexBuffers(vertexBuffer);
                effect.CurrentTechnique.Passes[0].Apply();
                Cube.device.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount/3);
            }
        }
    }
}
