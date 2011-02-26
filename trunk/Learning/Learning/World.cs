using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Learning
{
    class World
    {
        public Matrix partialWorld;
        public Matrix projection;
        public List<Player> players = new List<Player>();
        public List<Item> itemList = new List<Item>();
        public static GraphicsDevice device;
        public OctreeNode objectTree;
        Mapgen.Mapgen generator;

        public AIManager aiManager;

        public World(GraphicsDevice device)
        {
            World.device = device;
            Item[] req = {new Item(2,2), new Item(2,2)};
            Item[] req1 = { new Item(7, 2), new Item(7, 2) };
            Item[] req2 = { new Item(1, 2), new Item(1, 2) };
            Item[] req3 = { new Item(7, 1), null };
            Item[] req4 = { new Item(9, 2), new Item(9, 2) };
            Crafting.addRecipe(req, new Item(5, 1));
            Crafting.addRecipe(req1, new Item(6, 1));
            Crafting.addRecipe(req2, new Item(8, 1));
            Crafting.addRecipe(req3, new Item(9, 1));
            Crafting.addRecipe(req4, new Item(10, 1));
            // blocks are aligned on half integers rather than integers... make the octree be the same, hence the
            // origin of (0.5, 0.5, 0.5) rather than (0,0,0)
            OctreeNode.world = this;
            objectTree = new OctreeNode(new Vector3(0.5f, 0.5f, 0.5f), 20f, GameConstants.OctreeBlockLimit);
            generator = new Mapgen.Mapgen(objectTree);
            aiManager = new AIManager(this);
            generator.generateRock(10, 40);
            generator.generateLand(300);
        }
        public void saveGame(String location)
        {
            Stream stream = File.Open(location, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            GUI.print("Saving game to: " + location);
            bformatter.Serialize(stream, objectTree);
            stream.Close();
        }
        public void loadGame(String location)
        {
            Stream stream = File.Open(location, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            GUI.print("Loading game from: " + location);
            this.objectTree = (OctreeNode)bformatter.Deserialize(stream);
            stream.Close();
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

        public void Update(GameTime gameTime)
        {
            foreach (Player player in this.players)
            {
                //player.Update();
            }
            Item.Update(this);
            this.partialWorld = players[0].getCameraMatrix();// partialWorld;
            //objectTree.Update(gameTime);

        }
        public bool addBlock(Ray lookAt, int type)
        {
            return objectTree.addBlock(lookAt, type);
        }
        public void destroyBlock(Ray lookAt)
        {
            objectTree.destroyBlock(lookAt);
        }

        public void Draw()
        {
            BoundingFrustum toDraw = new BoundingFrustum(partialWorld * projection);
            objectTree.drawChild(toDraw);
            Item.Draw(this);
        }

        public void generateFloor()
        {
            for (int u = -10; u < 10; u++)
            {
                for (int v = -10; v < 10; v++)
                {
                    objectTree.addBlock(u, 0, v, 7);
                    objectTree.addBlock(u, 1, v, 7);
                    objectTree.addBlock(u, 2, v, 1);
                    objectTree.addBlock(u, 3, v, 3);
                    objectTree.addBlock(u, 4, v, 0);
                    objectTree.addBlock(u, 5, v, 2);
                    objectTree.addBlock(u, 6, v, 4);
                }
            }
            //EnemyAgent enemy = new EnemyAgent(new Vector3(5, 10, 5), this, aiManager);
            //objectTree.addObject(enemy);
            
        }

        #region Collision Detection
        public bool collisionCheck(List<GameObject> candidates, ref Vector3 endPos, ref bool onGround, ref Vector3 outsideVel)
        {
            BoundingBox actorAABB = new BoundingBox(
                endPos - GameConstants.PlayerSize / 2,
                endPos + GameConstants.PlayerSize / 2);
            // move the player's camera up
            actorAABB.Min.Y -= GameConstants.PlayerSize.Y / 3;
            actorAABB.Max.Y -= GameConstants.PlayerSize.Y / 3;
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if (!candidates[i].hitBox.Intersects(actorAABB))
                {
                    //candidates.RemoveAt(i);
                    continue;
                }
                Vector3 correction = getMinimumPenetrationVector(actorAABB, candidates[i].hitBox);
                endPos += correction;
                //actorAABB.Max += correction;
                //actorAABB.Min += correction;
                if (correction.Y != 0)
                {
                    outsideVel.Y = 0;
                    GUI.print("hit ground");
                    if (correction.Y > 0)
                    {
                        onGround = true;
                    }
                    if (correction.X != 0 && correction.Z != 0)
                    {
                        return true;
                        GUI.print("collided!");
                    }
                }
            }
            return false;
            //onGround = true;
            //outsideVel = Vector3.Zero;
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
