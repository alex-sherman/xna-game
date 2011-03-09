using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Learning.Graphics
{
    class Settings
    {
        public static Matrix projection;
        public static bool enableWater = true;
        public static int waterQuality = 5;
        public const String SandTexture = @"Textures\sand";
        public const String GrassTexture = @"Textures\grass";
        public const String RockTexture = @"Textures\rock";
        public const String WaterBumpMap = @"Textures\waterbump";
        
        public static void Init(GraphicsDevice device)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4.0f,
                (float)device.Viewport.Width /
                (float)device.Viewport.Height,
                1f, 800);
            Water.ResetTargets(waterQuality);
        }
        public static void setWaterQuality(int quality)
        {
            waterQuality = quality;
            Water.ResetTargets(quality);
        }
        
    }
}
