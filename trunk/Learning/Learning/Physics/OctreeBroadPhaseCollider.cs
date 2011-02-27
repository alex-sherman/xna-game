using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class OctreeBroadPhaseCollider : IBroadPhaseCollider
    {
        private PhysicsEngine _engine;

        public OctreeNode objectOctree;

        public OctreeBroadPhaseCollider(PhysicsEngine engine)
        {
            _engine = engine;
            objectOctree = new OctreeNode(Vector3.Zero, 500, GameConstants.OctreeBlockLimit);
        }

        public void Add(PhysicsObject obj)
        {
            objectOctree.addObject(obj);
        }

        public void Update()
        {
            
            throw new NotImplementedException();
        }

        public event BroadPhaseCollisionHandler OnBroadPhaseCollision;
    }
}
