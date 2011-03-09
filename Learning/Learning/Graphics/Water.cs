﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Learning.Graphics
{
    class Water : GenRend
    {
        public const float waterHeight = 18f;
        public static RenderTarget2D refractionRenderTarget;
        public static RenderTarget2D reflectionRenderTarget;
        public VertexBuffer waterV;
        World _world;
        public Water(int size,World world)
        {
            _world = world;
            int a = 20;
            waterV = new VertexBuffer(GraphicsEngine.device, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
            waterV.SetData(new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3(0,waterHeight,0), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(size*a,waterHeight,0), new Vector2(1,0)),
                new VertexPositionTexture(new Vector3(0,waterHeight,size*a), new Vector2(0,1)),
                new VertexPositionTexture(new Vector3(size*a,waterHeight,size*a), new Vector2(1,1))
                    });

        }
        public static void ResetTargets(int quality)
        {
            refractionRenderTarget = new RenderTarget2D(GraphicsEngine.device, (int)(GraphicsEngine.device.Viewport.Width * quality/5f), (int)(GraphicsEngine.device.Viewport.Height * quality/5f), false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            reflectionRenderTarget = new RenderTarget2D(GraphicsEngine.device, (int)(GraphicsEngine.device.Viewport.Width * quality/5f), (int)(GraphicsEngine.device.Viewport.Height * quality/5f), false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }
        public void save()
        {
            using (Stream stream = File.OpenWrite("cat.jpg"))
            {
                refractionRenderTarget.SaveAsJpeg(stream, refractionRenderTarget.Width, refractionRenderTarget.Height);
            }
            using (Stream stream = File.OpenWrite("bat.jpg"))
            {
                reflectionRenderTarget.SaveAsJpeg(stream, reflectionRenderTarget.Width, reflectionRenderTarget.Height);
            }
        }
        public override void Draw()
        {
            GraphicsEngine.DrawWater(this);
        }

    }
}
