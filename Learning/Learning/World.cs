using System;
using System.Collections;
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
        public ArrayList chunkList = new ArrayList();
        public ArrayList players = new ArrayList();
        public ArrayList itemList = new ArrayList();
        public static GraphicsDevice device;
        public World(GraphicsDevice device)
        {
            World.device = device;
        }
        public void spawnItem(int type, Vector3 position)
        {
            Item poo = new Item(position, type);
        }
        public void addChunk(int x, int y, int z)
        {
            this.chunkList.Add(new Chunk(new Vector3(x, y, z), this));
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
        public bool addBlock(Ray lookat, int type)
        {

            Chunk newChunk;
            foreach (Chunk chunk in this.chunkList)
            {
                if (lookat.Intersects(chunk.hitBox) != null)
                {
                    Block hit = chunk.getBlock(lookat);
                    if (hit != null)
                    {
                        Vector3 position = hit.getNormal(lookat) + hit.position;
                        newChunk = this.getChunk(position);
                        if (newChunk != null && ((Player)players[0]).hitBox.Contains(position) != ContainmentType.Contains)
                        {
                            return newChunk.addBlock(hit.getNormal(lookat) + hit.position - newChunk.position, type);
                        }
                    }
                }
            }
            return false;
        }
        public void destroyBlock(Ray lookat)
        {
            foreach (Chunk chunk in this.chunkList)
            {
                if (lookat.Intersects(chunk.hitBox) != null)
                {
                    Block destroyed = chunk.getBlock(lookat);
                    if (destroyed != null)
                    {
                        this.spawnItem(destroyed.type, destroyed.position);
                        chunk.destroyBlock(destroyed);
                        return;
                    }
                }
            }

        }
        public Chunk getChunk(float i, float j, float k)
        {
            Vector3 point = new Vector3(i, j, k);
            foreach (Chunk chunk in this.chunkList)
            {
                if (chunk.hitBox.Contains(point) == ContainmentType.Contains)
                {
                    return chunk;
                }
            }
            return null;
        }
        public Chunk getChunk(Vector3 pos)
        {
            float i = pos.X;
            float j = pos.Y;
            float k = pos.Z;
            foreach (Chunk chunk in this.chunkList)
            {
                if (chunk.position.X + 10 > i && chunk.position.X <= i &&
                    chunk.position.Y + 10 > j && chunk.position.Y <= j &&
                    chunk.position.Z + 10 > k && chunk.position.Z <= k)
                {
                    return chunk;
                }
            }
            return null;
        }
        public void Draw()
        {
            foreach (Chunk chunk in this.chunkList)
            {
                chunk.Draw();
            }
            Item.Draw(this);
        }

        #region Collision Detection
        public void collisionCheck(ref Vector3 endPos, ref bool onGround)
        {
            BoundingBox endAABB = new BoundingBox(
                endPos - GameConstants.PlayerSize / 2,
                endPos + GameConstants.PlayerSize / 2);

            // move the player's camera up a bit
            endAABB.Min.Y -= 1f;
            endAABB.Max.Y += 1f;

            foreach (Chunk chunk in this.chunkList)
            {
                foreach (Block b in chunk.BlockList)
                {
                    if (b != null)
                    {
                        Vector3 correction = getMinimumPenetrationVector(endAABB, b.hitBox);
                        endPos += correction;
                        if (correction.Y > 0) onGround = true;
                        if (!correction.Equals(Vector3.Zero)) return;
                    }
                }
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
