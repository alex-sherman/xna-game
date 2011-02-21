using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class World
    {
        public Matrix partialWorld;
        public Matrix projection;
        public List<Player> players = new List<Player>();
        public List<Item> itemList = new List<Item>();
        public static GraphicsDevice device;
        public OctreeNode blockTree;

        public World(GraphicsDevice device)
        {
            World.device = device;
            blockTree = new OctreeNode(this, Vector3.Zero, 500f, 16);
            generateFloor();
        }
        public void spawnItem(int type, Vector3 position)
        {
            Item poo = new Item(position, type);
        }
        public void addPlayer(Player player)
        {
            this.players.Add(player);
            player.world = this;
        }

        public void Update(Matrix partialWorld)
        {
            foreach (Player player in this.players)
            {
                //player.Update();
            }
            Item.Update(this);
            this.partialWorld = partialWorld;

        }
        public bool addBlock(Ray lookAt, int type)
        {
            return blockTree.addBlock(lookAt, type);
        }
        public void destroyBlock(Ray lookAt)
        {
            blockTree.destroyBlock(lookAt);
        }

        public void Draw()
        {
            BoundingFrustum toDraw = new BoundingFrustum(partialWorld * projection);
            blockTree.Draw(toDraw);
            Item.Draw(this);
        }

        public void generateFloor()
        {
            for (int u = -10; u < 10; u++)
            {
                for (int v = -10; v < 10; v++)
                {
                    blockTree.addBlock(u, 0, v, 1);
                    blockTree.addBlock(u, 1, v, 2);
                    blockTree.addBlock(u, 2, v, 3);
                    blockTree.addBlock(u, 3, v, 1);
                    blockTree.addBlock(u, 4, v, 4);
                }
            }
            blockTree.redistributeObjects();
        }

        #region Collision Detection
        public void collisionCheck(ref Vector3 endPos, ref bool onGround)
        {
            BoundingBox endAABB = new BoundingBox(
                endPos - GameConstants.PlayerSize / 2,
                endPos + GameConstants.PlayerSize / 2);

            // move the player's camera up a bit
            endAABB.Min.Y -= GameConstants.PlayerSize.Y / 2;
            endAABB.Max.Y += GameConstants.PlayerSize.Y / 2;
            foreach (Block b in blockTree.getCollisionCandidates(endAABB))
            {
                Vector3 correction = getMinimumPenetrationVector(endAABB, b.hitBox);
                endPos += correction;
                //endAABB.Max += correction;
                //endAABB.Min += correction;
                if (correction.Y > 0) onGround = true;
                if (!correction.Equals(Vector3.Zero)) return;
            }

        }

        /// <summary>
        /// Calculates the "minimum penetration" vector pointing from
        /// box2 to box1 - that is, the smallest distance needed to
        /// travel to free box1 from the collision. If they aren't colliding,
        /// returns (0,0,0).
        /// </summary>
        /// <param name="box1">The colliding bounding box</param>
        /// <param name="box2">The collided bounding box.</param>
        /// <returns>A trajectory for box1 to free it from the collision</returns>
        public Vector3 getMinimumPenetrationVector(BoundingBox box1, BoundingBox box2)
        {
            Vector3 result = Vector3.Zero;

            float diff, minDiff;
            int axis, side;

            // neg X
            diff = box1.Max.X - box2.Min.X;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            minDiff = diff;
            axis = 0;
            side = -1;

            // pos X
            diff = box2.Max.X - box1.Min.X;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                side = 1;
            }

            // neg Y
            diff = box1.Max.Y - box2.Min.Y;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 1;
                side = -1;
            }

            // pos Y
            diff = box2.Max.Y - box1.Min.Y;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 1;
                side = 1;
            }

            // neg Z
            diff = box1.Max.Z - box2.Min.Z;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 2;
                side = -1;
            }

            // pos Z
            diff = box2.Max.Z - box1.Min.Z;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 2;
                side = 1;
            }

            // Intersection occurred
            if (axis == 0)
                result.X = (float)side * minDiff;
            else if (axis == 1)
                result.Y = (float)side * minDiff;
            else
                result.Z = (float)side * minDiff;

            return result;
        }
        #endregion
    }
}
