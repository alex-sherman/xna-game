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
        public List<GameObject> visibleObjects = new List<GameObject>();
        public List<VertexPositionNormalTexture>[] vertices;
        public VertexBuffer[] vBuffers;
        public DynamicVertexBuffer[] instanceBuffers;
        public IndexBuffer[] iBuffers;
        public List<Matrix>[] instances;
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
                    bool addedToChild = false;
                    foreach (OctreeNode child in children)
                    {
                        if (child.bounds.Intersects(gameObjects[i].hitBox))
                        {
                            child.gameObjects.Add(gameObjects[i]);
                            addedToChild = true;
                        }
                    }
                    if (addedToChild) gameObjects.RemoveAt(i);
                }
                foreach (OctreeNode child in children)
                {
                    child.redistributeObjects();
                }
            }
        }

        public bool addObject(GameObject obj)
        {
            bool addedToChild = false;
            foreach (OctreeNode child in children)
            {
                if (child.bounds.Intersects(obj.hitBox))
                {
                    child.addObject(obj);
                    addedToChild = true;
                }
            }
            if (!addedToChild)
            {
                gameObjects.Add(obj);
                this.redistributeObjects();
            }

            return true;
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

    }
}
