﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class OctreeBroadPhaseCollider : IBroadPhaseCollider
    {
        private PhysicsEngine _engine;

        public PhysicsOctreeNode staticObjectTree;
        public List<PhysicsObject> dynamicObjects;

        public OctreeBroadPhaseCollider(PhysicsEngine engine)
        {
            _engine = engine;
            staticObjectTree = new PhysicsOctreeNode(_engine, Vector3.Zero, 500, GameConstants.OctreeBlockLimit);
        }

        public void Add(PhysicsObject obj)
        {
            if (obj.IsStatic)
                staticObjectTree.addObject(obj);
            else
                dynamicObjects.Add(obj);
        }

        public void Update()
        {
            foreach (PhysicsObject obj in dynamicObjects)
            {
                List<PhysicsObject> possibleCollisions = staticObjectTree.getCollisionCandidates(obj.hitBox);
                foreach (PhysicsObject obj2 in possibleCollisions)
                {
                    _engine.ActiveCollisions.AddPair(obj, obj2, _engine);
                }
            }
        }

        public event BroadPhaseCollisionHandler OnBroadPhaseCollision;
    }
}
