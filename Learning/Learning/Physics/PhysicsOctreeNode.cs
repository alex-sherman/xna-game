﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class PhysicsOctreeNode
    {
        #region Declarations

        private PhysicsEngine _engine;

        public List<PhysicsObject> Objects;
        public List<PhysicsOctreeNode> children;
        public PhysicsOctreeNode parent;
        public readonly BoundingBox bounds;
        public readonly Vector3 center;
        public readonly float nodeSize;
        public readonly int maxObjects;
        
        #endregion

        public PhysicsOctreeNode(PhysicsEngine engine, Vector3 center, float size, int maxObjects)
        {
            _engine = engine;
            this.center = center;
            this.nodeSize = size;
            this.maxObjects = maxObjects;
            //bounds = new BoundingBox();
            bounds.Min = center - new Vector3(size);
            bounds.Max = center + new Vector3(size);
            Objects = new List<PhysicsObject>();
            children = new List<PhysicsOctreeNode>();
        }

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
                            PhysicsOctreeNode newNode = new PhysicsOctreeNode(
                                _engine,
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
            if (Objects.Count > maxObjects)
            {
                splitTree();
                for (int i = Objects.Count - 1; i >= 0; i--)
                {
                    bool addedToChild = false;
                    foreach (PhysicsOctreeNode child in children)
                    {
                        if (child.bounds.Intersects(Objects[i].hitBox))
                        {
                            child.Objects.Add(Objects[i]);
                            addedToChild = true;
                        }
                    }
                    if (addedToChild) Objects.RemoveAt(i);
                }
                foreach (PhysicsOctreeNode child in children)
                {
                    child.redistributeObjects();
                }
            }
        }

        public bool addObject(PhysicsObject obj)
        {
            bool addedToChild = false;
            foreach (PhysicsOctreeNode child in children)
            {
                if (child.bounds.Intersects(obj.hitBox))
                {
                    child.addObject(obj);
                    addedToChild = true;
                }
            }
            if (!addedToChild)
            {
                Objects.Add(obj);
                this.redistributeObjects();
            }
            return true;
        }

        public List<PhysicsObject> getAllObjects()
        {
            List<PhysicsObject> result = new List<PhysicsObject>();
            result.AddRange(Objects);
            foreach (PhysicsOctreeNode child in children)
            {
                result.AddRange(child.getAllObjects());
            }
            return result;
        }
        public PhysicsOctreeNode getContainingNode(BoundingBox box)
        {
            foreach (PhysicsOctreeNode child in children)
            {
                Vector3 center = (box.Max + box.Min) / 2;
                if (child.bounds.Contains(center) == ContainmentType.Contains)
                {
                    return child.getContainingNode(box);
                }
            }
            return this;
        }
        public List<PhysicsObject> getCollisionCandidates(BoundingBox box)
        {
            return getContainingNode(box).Objects;
        }
    }
}
