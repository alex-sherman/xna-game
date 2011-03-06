using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class Voxel
    {
        private static Vector3 _extents = new Vector3(GameConstants.VoxelSize);
        internal AABB bounds;
        internal List<PhysicsObject> objects;

        public Voxel(PhysicsObject obj)
        {
            // obj is any object inside this voxel
            Vector3 min = obj.Position;
            min.X -= (int)(obj.Position.X) & (GameConstants.VoxelSize - 1);
            min.Y -= (int)(obj.Position.Y) & (GameConstants.VoxelSize - 1);
            min.Z -= (int)(obj.Position.Z) & (GameConstants.VoxelSize - 1);

            bounds.Min = min;
            bounds.Max = min + _extents;
            objects = new List<PhysicsObject>();
        }

        public void Add(PhysicsObject obj)
        {
            objects.Add(obj);
        }
    }

    class HashBucket
    {
        internal HashBucket next = null;
        internal Voxel voxel = null;

        public HashBucket() { }

        public bool Add(PhysicsObject obj)
        {
            // currently stores objects in only one box - the one containing their Min point.
            // uses sideways chaining to resolve hash collisions
            if (voxel == null)
            {
                voxel = new Voxel(obj);
            }
            if (voxel.bounds.Contains(obj.hitBox.Min) == ContainmentType.Contains)
            {
                voxel.Add(obj);
                return true;
            }
            else if (next != null)
            {
                return next.Add(obj);
            }
            else
            {
                next = new HashBucket();
                return next.Add(obj);
            }
        }
    }

    class HashVoxelBroadPhase
    {
        private HashBucket[] _table;

        // large prime numbers for hashing
        const UInt32 h1 = 0x8da6b343;
        const UInt32 h2 = 0xd8163841;
        const UInt32 h3 = 0xcb1ab31f;

        public HashVoxelBroadPhase()
        {
            _table = new HashBucket[GameConstants.NumHashBuckets];
        }

        public void Add(PhysicsObject obj)
        {
            int index = GetIndex(obj);
            if (_table[index] == null)
            {
                _table[index] = new HashBucket();
            }
            _table[index].Add(obj);
        }

        public int GetIndex(PhysicsObject obj)
        {
            // The number of hash buckets must be a power of two!
            // really : indexX = floor(obj.Position.X / VoxelSize) % NumBuckets
            Int32 indexX = (int)Math.Floor(obj.Position.X * GameConstants.InverseVoxelSize) & (GameConstants.NumHashBuckets - 1);
            Int32 indexY = (int)Math.Floor(obj.Position.Y * GameConstants.InverseVoxelSize) & (GameConstants.NumHashBuckets - 1);
            Int32 indexZ = (int)Math.Floor(obj.Position.Z * GameConstants.InverseVoxelSize) & (GameConstants.NumHashBuckets - 1);

            // make sure they're all > 0
            if (indexX < 0) indexX += GameConstants.NumHashBuckets;
            if (indexY < 0) indexY += GameConstants.NumHashBuckets;
            if (indexZ < 0) indexZ += GameConstants.NumHashBuckets;

            UInt32 tableIndex = h1 * (UInt32)indexX + h2 * (UInt32)indexY + h3 * (UInt32)indexZ;
            tableIndex &= (GameConstants.NumHashBuckets - 1);
            return (int)tableIndex;
        }
    }
}
