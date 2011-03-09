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
        
        public static void DrawWorld(List<GenRend> scene, List<GenRend> water)
        {
            Color poopy = Color.CornflowerBlue;
            poopy.A = (byte).01f;


            UpdateWater(Graphics.Water.reflectionRenderTarget, Graphics.Water.refractionRenderTarget, scene);
            GraphicsEngine.device.Clear(poopy);
            effect.Parameters["view"].SetValue(world.view);
            effect.CurrentTechnique = effect.Techniques["MultiTexture"];
            Render(scene);
            Render(water);
        }
        public static void setBuffers(VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            device.SetVertexBuffers(vertexBuffer);
            device.Indices = indexBuffer;
        }
        public static void Draw(ModelRend modelRend)
        {
            foreach (ModelMesh mesh in modelRend.model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = modelRend.getWorld();
                    effect.View = GraphicsEngine.effect.Parameters["view"].GetValueMatrix();
                    effect.Projection = Graphics.Settings.projection;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
        public static void DrawWater(Graphics.Water water)
        {
            setBuffers(water.waterV, null);
            SetWorld(Matrix.CreateTranslation(new Vector3(-800, 0, -800)));
            effect.Parameters["aboveWater"].SetValue(world.players[0].Position.Y > Graphics.Water.waterHeight);
            effect.Parameters["cameraPosition"].SetValue(world.players[0].Position);
            effect.Parameters["view"].SetValue(world.view);
            effect.Parameters["reflView"].SetValue(world.reflectionView);
            effect.CurrentTechnique = effect.Techniques["WaterEffect"];
            effect.Parameters["Reflection"].SetValue(Graphics.Water.reflectionRenderTarget);
            effect.Parameters["Refraction"].SetValue(Graphics.Water.refractionRenderTarget);
            effect.Parameters["time"].SetValue(time);
            _draw(2);
        }
        public static void UpdateWater(RenderTarget2D reflection, RenderTarget2D refraction, List<GenRend> scene)
        {
            Color poopy = Color.CornflowerBlue;
            poopy.A = (byte).01f;
            effect.CurrentTechnique = effect.Techniques["MultiTextureClip"];
            effect.Parameters["view"].SetValue(world.view);
            if(world.players[0].Position.Y > Graphics.Water.waterHeight)
                effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, -1, 0, Graphics.Water.waterHeight));
            else
                effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -Graphics.Water.waterHeight));
            device.SetRenderTarget(refraction);
            GraphicsEngine.device.Clear(poopy);
            Render(scene);
            effect.Parameters["view"].SetValue(world.reflectionView);
            effect.Parameters["ClipPlane1"].SetValue(new Vector4(0, 1, 0, -Graphics.Water.waterHeight));
            device.SetRenderTarget(reflection);
            GraphicsEngine.device.Clear(poopy);
            Render(scene);
            device.SetRenderTarget(null);
        }

        public static void Render()
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
        }
        public static void Render(Renderable renderable)
        {
            if (renderable.Visible(world.viewFrustrum))
            {
                SetWorld(renderable.getWorld());
                setBuffers(renderable.getVbuffer(), renderable.getIbuffer());
                Render();
            }
        }
        public static void Render(GenRend renderable)
        {
            if (renderable.Visible(world.viewFrustrum)) { renderable.Draw(); }
        }
        public static void Render(List<Renderable> scene)
        {
            foreach (Renderable facet in scene) { facet.Draw(); }
        }
        public static void Render(List<GenRend> scene)
        {
            foreach (GenRend facet in scene) { facet.Draw(); }
        }
        public static void _draw(int count)
        {
            effect.Parameters["proj"].SetValue(Graphics.Settings.projection);
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, count);
            
        }
    }
}
