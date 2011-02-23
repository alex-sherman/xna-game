using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Learning
{
    [Serializable()]
    class OctreeNode : ISerializable
    {
        #region Declarations
        public List<GameObject> gameObjects;
        public List<OctreeNode> children;
        public static World world;
        public OctreeNode parent;
        public readonly BoundingBox bounds;
        public readonly Vector3 center;
        public readonly float nodeSize;
        public readonly int maxObjects;
        #endregion

        public OctreeNode(Vector3 center, float size, int maxObjects)
        {
            this.center = center;
            this.nodeSize = size;
            this.maxObjects = maxObjects;
            bounds = new BoundingBox();
            bounds.Min = center - new Vector3(size);
            bounds.Max = center + new Vector3(size);
            gameObjects = new List<GameObject>();
            children = new List<OctreeNode>();
        }
        #region Serialization
        public OctreeNode(SerializationInfo info, StreamingContext context)
        {
            this.center = (Vector3)info.GetValue("center", typeof(Vector3));
            this.nodeSize = (float)info.GetValue("nodeSize", typeof(float));
            this.maxObjects = (int)info.GetValue("maxObjects", typeof(int));
            this.parent = (OctreeNode)info.GetValue("parent", typeof(OctreeNode));
            bounds = (BoundingBox)info.GetValue("bounds", typeof(BoundingBox));
            gameObjects = (List<GameObject>)info.GetValue("blocks", typeof(List<GameObject>));
            children = (List<OctreeNode>)info.GetValue("children", typeof(List<OctreeNode>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("center", this.center);
            info.AddValue("nodeSize", this.nodeSize);
            info.AddValue("maxObjects", this.maxObjects);
            info.AddValue("parent", this.parent);
            info.AddValue("bounds", this.bounds);
            info.AddValue("gameObjects", this.gameObjects);
            info.AddValue("children", this.children);
        }
        #endregion

        protected void splitTree()
        {
            if (children.Count == 0)
            {
                float childSize = nodeSize / 2.0f;

                Vector3 offsetX = Vector3.UnitX * childSize;
                Vector3 offsetY = Vector3.UnitY * childSize;
                Vector3 offsetZ = Vector3.UnitZ * childSize;
                for (int x = -1; x <= 1; x += 2)
                {
                    for (int y = -1; y <= 1; y += 2)
                    {
                        for (int z = -1; z <= 1; z += 2)
                        {
                            OctreeNode newNode = new OctreeNode(
                                center + x * offsetX + y * offsetY + z * offsetZ,
                                childSize,
                                maxObjects);
                            newNode.parent = this;
                            children.Add(newNode);
                        }
                    }
                }
            }
        }
        public void redistributeObjects()
        {
            if (gameObjects.Count > maxObjects)
            {
                splitTree();
                for (int i = gameObjects.Count - 1; i >= 0; i--)
                {
                    foreach (OctreeNode child in children)
                    {
                        if (child.bounds.Contains(gameObjects[i].hitBox) == ContainmentType.Contains)
                        {
                            child.gameObjects.Add(gameObjects[i]);
                            gameObjects.RemoveAt(i);
                            break;
                        }
                    }
                }
                foreach (OctreeNode child in children)
                {
                    child.redistributeObjects();
                }
            }
        }

        public bool addObject(GameObject obj)
        {
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Contains(obj.hitBox) == ContainmentType.Contains)
                {
                    return child.addObject(obj);
                }
            }
            gameObjects.Add(obj);
            this.redistributeObjects();
            return true;
        }
        public void addBlock(Vector3 pos, int type)
        {
            Block poo = new Block(pos, type);
            addObject(poo);
        }
        public bool addBlock(int x, int y, int z, int type)
        {
            Block poo = new Block(new Vector3(x, y, z), type);
            return addObject(poo);
        }
        public bool addBlock(Ray lookAt, int type)
        {
            OctreeNode containingNode;
            GameObject target = null;
            target = getBlock(lookAt, out containingNode);
            if (target == null || target.GetType() != typeof(Block))
            {
                return false;
            }
            Vector3 position = ((Block)target).getNormal(lookAt) + target.Position;
            if (OctreeNode.world.players[0].hitBox.Contains(position) != ContainmentType.Contains)
            {
                addBlock(position, type);
                return true;
            }

            return false;
        }
        public GameObject getBlock(Ray lookAt, out OctreeNode containingNode)
        {
            float junk;
            OctreeNode node;
            GameObject result = getBlock(lookAt, out junk, out node);
            if (result != null)
            {
                containingNode = node;
                return result;
            }
            containingNode = null;
            return null;
        }
        /// <summary>
        /// Internal method that returns the closest block intersected by lookAt
        /// as well as the distance to that block (in the variable dist)
        /// </summary>
        /// <param name="lookAt">A ray to trace</param>
        /// <param name="maxDist">Out: the distance to the returned block.</param>
        /// <returns>The closest intersected block</returns>
        protected GameObject getBlock(Ray lookAt, out float dist, out OctreeNode containing)
        {
            float minDist = float.PositiveInfinity;
            GameObject closest = null;
            OctreeNode containingNode = null;
            foreach (GameObject obj in gameObjects)
            {
                float? distance = lookAt.Intersects(obj.hitBox);
                if (distance != null && distance < minDist)
                {
                    closest = obj;
                    minDist = (float)distance;
                    containingNode = this;
                }
            }
            foreach (OctreeNode child in children)
            {
                float childDist;
                OctreeNode node;
                GameObject hit = child.getBlock(lookAt, out childDist, out node);
                if (childDist < minDist)
                {
                    minDist = childDist;
                    closest = hit;
                    containingNode = node;
                }
            }
            dist = minDist;
            containing = containingNode;
            return closest;
        }
        public bool destroyBlock(Ray lookAt)
        {
            OctreeNode containingNode;
            GameObject destroyed = getBlock(lookAt, out containingNode);
            if (destroyed == null || destroyed.GetType() != typeof(Block))
            {
                return false;
            }
            OctreeNode.world.spawnItem(((Block)destroyed).type, destroyed.Position);
            containingNode.gameObjects.Remove(destroyed);
            return true;
        }

        public void Draw(BoundingFrustum boundingFrustum)
        {
            List<Block> drawLast = new List<Block>();

            foreach (GameObject obj in gameObjects)
            {
                if (obj.GetType() == typeof(Block) && ((Block)obj).type == 4)
                    drawLast.Add((Block)obj);
                else obj.Draw(OctreeNode.world);
            }
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Intersects(boundingFrustum))
                    drawLast.AddRange(child.drawChild(boundingFrustum));
            }
            foreach (Block block in drawLast)
            {
                block.Draw(OctreeNode.world);
            }
        }
        /// <summary>
        /// Draws all blocks contained in the current octree node except for those of type 4;
        /// instead, it returns those in a list.
        /// </summary>
        /// <param name="boundingFrustum">The frustum containing the player's view</param>
        /// <returns>A list of all type 4 blocks in the node</returns>
        public List<Block> drawChild(BoundingFrustum boundingFrustum)
        {
            List<Block> drawLast = new List<Block>();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetType() == typeof(Block) && ((Block)gameObject).type == 4)
                    drawLast.Add((Block)gameObject);

                else gameObject.Draw(OctreeNode.world);
            }

            foreach (OctreeNode child in children)
            {
                if (child.bounds.Intersects(boundingFrustum))
                    drawLast.AddRange(child.drawChild(boundingFrustum));
            }

            return drawLast;
        }

        public List<GameObject> getAllBlocks()
        {
            List<GameObject> result = new List<GameObject>();
            result.AddRange(gameObjects);
            foreach (OctreeNode child in children)
            {
                result.AddRange(child.getAllBlocks());
            }
            return result;
        }
        public OctreeNode getContainingNode(BoundingBox box)
        {
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Contains(box) == ContainmentType.Contains)
                {
                    return child.getContainingNode(box).parent;
                }
            }
            return this;
        }
        public List<GameObject> getCollisionCandidates(BoundingBox box)
        {
            OctreeNode containingNode = getContainingNode(box);
            return containingNode.getAllBlocks();
        }

        public List<GameObject> getMobileObjects()
        {
            List<GameObject> result = new List<GameObject>();
            foreach (GameObject obj in gameObjects)
            {
                if (obj.GetType() != typeof(Block))
                {
                    result.Add(obj);
                }
            }
            foreach (OctreeNode child in children)
            {
                result.AddRange(child.getMobileObjects());
            }
            return result;
        }

        public Block getBlockAtPoint(Vector3 vec) {
            // assumes vec is the center of a block of width 1
            Vector3 diagonal = new Vector3(0.5f);
            BoundingBox box = new BoundingBox(vec - diagonal, vec + diagonal);
            foreach (OctreeNode child in children) {
                if (child.bounds.Contains(box) == ContainmentType.Contains) {
                    return child.getBlockAtPoint(vec);
                }
            }
            foreach (GameObject obj in gameObjects) {
                if (obj.GetType() != typeof(Block))
                    continue;
                if (obj.hitBox.Contains(vec) == ContainmentType.Contains)
                    return (Block)obj;
            }
            return null;
        }

        public List<Block> getNeighborBlocks(Block block)
        {
            List<Block> result = new List<Block>();
            // create a list of points of unit distance from block in each direction
            List<Vector3> points = new List<Vector3>();
            points.Add(block.Position + Vector3.UnitX);
            points.Add(block.Position - Vector3.UnitX);
            points.Add(block.Position + Vector3.UnitY);
            points.Add(block.Position - Vector3.UnitY);
            points.Add(block.Position + Vector3.UnitZ);
            points.Add(block.Position - Vector3.UnitZ);

            // now search the tree for any blocks at these locations
            foreach (Vector3 vec in points)
            {
                Block b = getBlockAtPoint(vec);
                if (b != null)
                    result.Add(b);
            }
            return result;
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject obj in getMobileObjects())
            {
                obj.Update(gameTime);
            }
        }
    }
}
