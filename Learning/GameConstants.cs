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
        #region Player constants

        public const float PlayerRunSpeed = 0.008f;
        public const float PlayerWalkSpeed = 0.004f;
        public const float Gravity = -5.0f;
        public const float PlayerJumpSpeed = 5.0f;
        public static Vector3 PlayerSize = new Vector3(0.6f, 1.5f, 0.6f);
        #endregion

        #region User interface variables
        public static Keys quickSaveKey = Keys.F5;
        public static Keys quickLoadKey = Keys.F6;
        #endregion

        #region Hash Voxel parameters

        public const int LogTwoVoxelSize = 3; // 3 -> 8x8x8 voxels
        public const int LogTwoNumHashBuckets = 10; // 10 -> 1024 buckets

        // don't change these two
        public const int VoxelSize = (1 << LogTwoVoxelSize);
        public const int NumHashBuckets = (1 << LogTwoNumHashBuckets);
        public const float InverseVoxelSize = 1f / VoxelSize;

        #endregion

        #region Misc.
        public const int OctreeBlockLimit = 64;
        #endregion

    }
}