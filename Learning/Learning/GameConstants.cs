using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Learning
{
    class GameConstants
    {
        // Player constants
        public const float PlayerRunSpeed = 0.008f;
        public const float PlayerWalkSpeed = 0.004f;
        public const float Gravity = 0.0002f;
        public const float PlayerJumpSpeed = .005f;
        public static Vector3 PlayerSize = new Vector3(0.6f, 1.5f, 0.6f);

        // Interface variables.
        public static Keys quickSaveKey = Keys.F5;
        public static Keys quickLoadKey = Keys.F6;

        // Misc.
        public const int OctreeBlockLimit = 256;
    }
}