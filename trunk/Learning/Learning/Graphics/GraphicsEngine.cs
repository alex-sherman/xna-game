using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Learning
{
    class GraphicsEngine
    {
        public static Effect effect;
        public static GraphicsDevice device;
        public static World world;
        public static float time;
        public static bool wireFrame = false;

        public static void Initialize(ScreenManager screen, ContentManager Content)
        {
            Graphics.Settings.Init(screen.GraphicsDevice);

            effect = Content.Load<Effect>("LightAndTextureEffect");
            effect.Parameters["ambientLightColor"].SetValue(
                    Color.White.ToVector4() * .6f);
            effect.Parameters["diffuseLightColor"].SetValue(
                Color.White.ToVector4());
            effect.Parameters["specularLightColor"].SetValue(
                Color.White.ToVector4() / 3);
            effect.Parameters["lightPosition"].SetValue(
                    new Vector3(0f, 10f, 10f));
            effect.Parameters["specularPower"].SetValue(12f);
            effect.Parameters["specularIntensity"].SetValue(.5f);

            GraphicsEngine.device = screen.GraphicsDevice;
            effect.Parameters["UserTextureA"].SetValue(Content.Load<Texture2D>(Graphics.Settings.SandTexture));
            effect.Parameters["UserTextureB"].SetValue(Content.Load<Texture2D>(Graphics.Settings.GrassTexture));
            effect.Parameters["UserTextureC"].SetValue(Content.Load<Texture2D>(Graphics.Settings.RockTexture));
            effect.Parameters["WaterBump"].SetValue(Content.Load<Texture2D>(Graphics.Settings.WaterBumpMap));

            Vector2 windDirection = (new Vector2(1, 2));
            windDirection.Normalize();
            effect.Parameters["windDirection"].SetValue(windDirection);
            
            
        }
        public static void SetWorld(Matrix location)
        {
            effect.Parameters["world"].SetValue(location);
        }
        public static void Draw(Graphics.Renderable renderable)
        {
            SetWorld(renderable.getWorld());
            setBuffers(renderable.getVbuffer(), renderable.getIbuffer());
        }
        public static void Draw(Landchunk land)
        {
            setBuffers(land.getVbuffer(), land.getIbuffer());
            SetWorld(land.getWorld());
            UpdateWater(land.water.reflectionRenderTarget, land.water.refractionRenderTarget);
            DrawLand();
            setBuffers(land.water.waterV, null);
            DrawWater(land.water.reflectionRenderTarget, land.water.refractionRenderTarget);
        }
        public static void setBuffers(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            device.SetVertexBuffers(vertexBuffer);
            device.Indices = indexBuffer;
        }
        public static void DrawLand()
        {
            effect.Parameters["view"].SetValue(world.view);
            effect.CurrentTechnique = effect.Techniques["MultiTexture"];
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -Graphics.Water.waterHeight));
            _draw(null);
        }
        public static void DrawWater(Texture2D reflection, Texture2D refraction)
        {
            effect.Parameters["aboveWater"].SetValue(world.players[0].Position.Y > Graphics.Water.waterHeight);
            effect.Parameters["cameraPosition"].SetValue(world.players[0].Position);
            effect.Parameters["view"].SetValue(world.view);
            effect.Parameters["reflView"].SetValue(world.reflectionView);
            effect.CurrentTechnique = effect.Techniques["WaterEffect"];
            effect.Parameters["Reflection"].SetValue(reflection);
            effect.Parameters["Refraction"].SetValue(refraction);
            effect.Parameters["time"].SetValue(time);
            _draw(2);
        }
        public static void UpdateWater(RenderTarget2D reflection, RenderTarget2D refraction)
        {
            effect.CurrentTechnique = effect.Techniques["MultiTextureClip"];
            effect.Parameters["view"].SetValue(world.view);
            if(world.players[0].Position.Y > Graphics.Water.waterHeight)
                effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, -1, 0, Graphics.Water.waterHeight));
            else
                effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -Graphics.Water.waterHeight));
            _draw(refraction);
            effect.Parameters["view"].SetValue(world.reflectionView);
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -Graphics.Water.waterHeight));
            
            effect.Parameters["world"].SetValue(Matrix.CreateTranslation(new Vector3(0, 0, 0)));
            _draw(reflection);
        }
        public static void _draw(RenderTarget2D target)
        {
            RasterizerState poo = new RasterizerState();

            if (wireFrame)
            {
                poo.FillMode = FillMode.WireFrame;
            }
            poo.CullMode = CullMode.None;
            poo.MultiSampleAntiAlias = false;
            device.RasterizerState = poo;
            int IndexCount = device.Indices.IndexCount;
            if (IndexCount>0)
            {
                effect.Parameters["proj"].SetValue(Graphics.Settings.projection);
                device.SetRenderTarget(target);
                Color poopy = Color.CornflowerBlue;
                poopy.A = (byte).01f;
                GraphicsEngine.device.Clear(poopy);
                effect.CurrentTechnique.Passes[0].Apply();

                for (int j = 0; j <= IndexCount / 3000000; j += 1)
                {
                    if ((j + 1) * 3000000 >= IndexCount)
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, IndexCount - j * 3000000, j * 3000000, (IndexCount - j * 3000000) / 3);
                        break;
                    }
                    else
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, j * 3000000, 3000000, j * 3000000, (3000000) / 3);

                    }
                }
            }
            device.SetRenderTarget(null);
        }
        public static void _draw(int count)
        {
            effect.Parameters["proj"].SetValue(Graphics.Settings.projection);
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, count);
            
        }
    }
}
