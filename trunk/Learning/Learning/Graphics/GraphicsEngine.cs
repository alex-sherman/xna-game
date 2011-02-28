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
        public static void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Texture2D texture)
        {
            if (vertexBuffer != null && indexBuffer != null)
            {
                effect.CurrentTechnique = effect.Techniques["Texture"];
                effect.CurrentTechnique.Passes[0].Apply();
                effect.Parameters["view"].SetValue(world.partialWorld);
                effect.Parameters["proj"].SetValue(world.projection);
                effect.Parameters["UserTexture"].SetValue(texture);
                Cube.device.SetVertexBuffers(vertexBuffer);
                Cube.device.Indices = indexBuffer;

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
        public static void Draw(VertexBuffer vertexBuffer, Texture2D texture)
        {
            if (vertexBuffer != null)
            {
                effect.CurrentTechnique = effect.Techniques["Texture"];
                effect.CurrentTechnique.Passes[0].Apply();
                effect.Parameters["view"].SetValue(world.partialWorld);
                effect.Parameters["proj"].SetValue(world.projection);
                effect.Parameters["UserTexture"].SetValue(texture);
                Cube.device.SetVertexBuffers(vertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }
        /*public static void Draw(Effect effect, Matrix[] instances)
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
        }*/
    }
}
