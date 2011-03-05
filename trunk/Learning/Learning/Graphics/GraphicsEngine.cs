using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class GraphicsEngine
    {
        public static Effect effect;
        public static GraphicsDevice device;
        public static World world;
        public static Texture2D grass;
        public static Texture2D sand;
        public static Texture2D rock;
        public static Texture2D water;
        public static void SetTextures(Texture2D grass, Texture2D sand, Texture2D rock,Texture2D water)
        {
            GraphicsEngine.grass = grass;
            GraphicsEngine.sand = sand;
            GraphicsEngine.rock = rock;
            GraphicsEngine.water = water;
        }
        public static void Initialize(GraphicsDevice device, Effect effect)
        {
            GraphicsEngine.device = device;
            GraphicsEngine.effect = effect;
            effect.Parameters["UserTextureA"].SetValue(sand);
            effect.Parameters["UserTextureB"].SetValue(grass);
            effect.Parameters["UserTextureC"].SetValue(rock);
            effect.Parameters["UserTextureD"].SetValue(grass);
            effect.Parameters["WaterBump"].SetValue(water);
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, -1, 0, 10));
            effect.Parameters["ClipPlane2"].SetValue(new Vector4(0, 1, 0, -10));
            DepthStencilState foo = new DepthStencilState();
            
            
        }
        public static void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            effect.Parameters["view"].SetValue(world.view);
            effect.CurrentTechnique = effect.Techniques["MultiTextureClip"];
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -10));
            device.SetVertexBuffers(vertexBuffer);
            device.Indices = indexBuffer;
            _draw(vertexBuffer, indexBuffer);

        }
        public static void DrawWater(VertexBuffer vertexBuffer, Texture2D reflection, Texture2D refraction)
        {
            effect.Parameters["cameraPosition"].SetValue(world.players[0].Position);
            effect.Parameters["view"].SetValue(world.view);
            effect.Parameters["reflView"].SetValue(world.reflectionView);
            device.SetVertexBuffers(vertexBuffer);
            effect.CurrentTechnique = effect.Techniques["WaterEffect"];
            effect.Parameters["Reflection"].SetValue(reflection);
            effect.Parameters["Refraction"].SetValue(refraction);
            _draw(2);
        }
        public static void UpdateWater(VertexBuffer vertexBuffer, IndexBuffer indexBuffer,RenderTarget2D reflection, RenderTarget2D refraction)
        {
            //device.SetVertexBuffers(vertexBuffer);
            //device.Indices = indexBuffer;
            effect.CurrentTechnique = effect.Techniques["MultiTextureClip"];
            effect.Parameters["view"].SetValue(world.view);
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, -1, 0, 10));
            _draw(vertexBuffer, indexBuffer, refraction);
            effect.Parameters["view"].SetValue(world.reflectionView);
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -10));
            _draw(vertexBuffer, indexBuffer,  reflection );
        }
        public static void _draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            _draw(vertexBuffer, indexBuffer, null);
        }
        public static void _draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, RenderTarget2D target)
        {
            RasterizerState poo = new RasterizerState();
            //poo.FillMode = FillMode.WireFrame;
            poo.CullMode = CullMode.None;
            poo.MultiSampleAntiAlias = false;
            device.RasterizerState = poo;
            if (vertexBuffer != null && indexBuffer != null)
            {
                
                
                effect.Parameters["proj"].SetValue(world.projection);
                device.SetRenderTarget(target);
                GraphicsEngine.device.Clear(Color.CornflowerBlue);
                effect.CurrentTechnique.Passes[0].Apply();

                for (int j = 0; j <= indexBuffer.IndexCount / 3000000; j += 1)
                {
                    if ((j + 1) * 3000000 >= indexBuffer.IndexCount)
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, indexBuffer.IndexCount - j * 3000000, j * 3000000, (indexBuffer.IndexCount - j * 3000000) / 3);
                        break;
                    }
                    else
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, 3000000, j * 3000000, (3000000) / 3);

                    }
                }   
                
                device.SetRenderTarget(null);
            }
        }
        public static void _draw(int count)
        {
            RasterizerState poo = new RasterizerState();
            poo.FillMode = FillMode.WireFrame;
            //device.RasterizerState = poo;
            effect.Parameters["view"].SetValue(world.view);
            effect.Parameters["proj"].SetValue(world.projection);
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, count);
            
        }
    }
}
