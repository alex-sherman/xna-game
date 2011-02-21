using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning
{
    class GameConstants
    {
        // Player variables
        public const float PlayerRunSpeed = 0.012f;
        public const float PlayerWalkSpeed = 0.008f;
        public const float Gravity = 0.00018f;
        public const float PlayerJumpSpeed = .005f;
        public static Vector3 PlayerSize = new Vector3(0.6f, 1.5f, 0.6f);
        public const float PlayerEyeFraction = 0.9f;

    }
}
