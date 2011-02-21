using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class OctreeNode
    {
        #region Declarations
        public List<Block> blocks = new List<Block>();
        public List<OctreeNode> children = new List<OctreeNode>();
        public World world;
        public OctreeNode parent;
        public BoundingBox bounds;
        public Vector3 center;
        public float size;
        public int maxObjects;
        #endregion

        public OctreeNode(World world, Vector3 center, float size, int maxObjects)
        {
            this.world = world;
            this.center = center;
            this.size = size;
            this.maxObjects = maxObjects;
            bounds = new BoundingBox();
            bounds.Min = center - new Vector3(size, size, size);
            bounds.Max = center + new Vector3(size, size, size);
        }
        protected void splitTree()
        {
            if (children.Count == 0)
            {
                float childSize = size / 2.0f;
                Vector3 offsetX = Vector3.UnitX * childSize / 2;
                Vector3 offsetY = Vector3.UnitY * childSize / 2;
                Vector3 offsetZ = Vector3.UnitZ * childSize / 2;
                children.Add(new OctreeNode(world, center - offsetX - offsetY - offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center - offsetX - offsetY + offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center - offsetX + offsetY - offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center - offsetX + offsetY + offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center + offsetX - offsetY - offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center + offsetX - offsetY + offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center + offsetX + offsetY - offsetZ, childSize, maxObjects));
                children.Add(new OctreeNode(world, center + offsetX + offsetY + offsetZ, childSize, maxObjects));
            }
        }
        public void redistributeObjects()
        {
            if (blocks.Count > maxObjects)
            {
                splitTree();
                for (int i = blocks.Count - 1; i >= 0; i--)
                {
                    Block block = blocks[i];
                    foreach (OctreeNode child in children)
                    {
                        if (child.bounds.Contains(block.hitBox) == ContainmentType.Contains)
                        {
                            child.blocks.Add(block);
                            blocks.Remove(block);
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
        public bool addBlock(Block block)
        {
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Contains(block.hitBox) == ContainmentType.Contains)
                {
                    return child.addBlock(block);
                }
            }
            blocks.Add(block);
            redistributeObjects();
            return true;
        }
        public void addBlock(Vector3 pos, int type)
        {
            Block poo = new Block(pos, type);
            addBlock(poo);
        }
        public bool addBlock(int x, int y, int z, int type)
        {
            Block poo = new Block(new Vector3(x, y, z), type);
            return addBlock(poo);
        }
        public bool addBlock(Ray lookAt, int type)
        {
            OctreeNode containingNode;
            Block target = null;
            target = getBlock(lookAt, out containingNode);
            if (target == null)
            {
                return false;
            }
            Vector3 position = target.getNormal(lookAt) + target.position;
            if (world.players[0].hitBox.Contains(position) != ContainmentType.Contains)
            {
                addBlock(position, type);
                return true;
            }

            return false;
        }
        public Block getBlock(Ray lookAt, out OctreeNode containingNode)
        {
            float junk;
            OctreeNode node;
            Block result = getBlock(lookAt, out junk, out node);
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
        protected Block getBlock(Ray lookAt, out float dist, out OctreeNode containing)
        {
            float minDist = float.PositiveInfinity;
            Block closest = null;
            OctreeNode containingNode = null;
            foreach (Block block in blocks)
            {
                float? distance = lookAt.Intersects(block.hitBox);
                if (distance != null && distance < minDist)
                {
                    closest = block;
                    minDist = (float)distance;
                    containingNode = this;
                }
            }
            foreach (OctreeNode child in children)
            {
                float childDist;
                OctreeNode node;
                Block hit = child.getBlock(lookAt, out childDist, out node);
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
            Block destroyed = getBlock(lookAt, out containingNode);
            if (destroyed == null)
            {
                return false;
            }
            world.spawnItem(destroyed.type, destroyed.position);
            containingNode.blocks.Remove(destroyed);
            return true;
        }
        public void Draw(BoundingFrustum boundingFrustum)
        {
            List<Block> drawLast = new List<Block>();

            //Cube.Draw(center, world, true, size);
            foreach (Block block in blocks)
            {
                if (block.type == 4)
                    drawLast.Add(block);
                else block.Draw(world);
            }
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Intersects(boundingFrustum))
                    drawLast.AddRange(child.drawChild(boundingFrustum));
            }
            foreach (Block block in drawLast)
            {
                block.Draw(world);
            }
        }
        public List<Block> drawChild(BoundingFrustum boundingFrustum)
        {
            List<Block> drawLast = new List<Block>();
            foreach (Block block in blocks)
            {
                if (block.type == 4)
                    drawLast.Add(block);
                else block.Draw(world);
            }
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Intersects(boundingFrustum))
                    drawLast.AddRange(child.drawChild(boundingFrustum));
            }
            return drawLast;
        }
        public List<Block> getAllBlocks()
        {
            List<Block> result = new List<Block>();
            result.AddRange(blocks);
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
                    return child.getContainingNode(box);
                }
            }
            return this;
        }
        public List<Block> getCollisionCandidates(BoundingBox box)
        {
            OctreeNode containingNode = getContainingNode(box);
            return containingNode.getAllBlocks();
        }
    }
}
