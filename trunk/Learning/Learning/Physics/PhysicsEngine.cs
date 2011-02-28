using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class PhysicsEngine
    {
        internal List<PhysicsObject> _objects;
        internal IBroadPhaseCollider _broadPhaseCollider;
        internal INarrowPhaseCollider _narrowPhaseCollider;
        internal Pool<CollisionPair> collisionPool;
        internal float _gravity;

        public CollisionList ActiveCollisions;

        public int Iterations = 5;

        public PhysicsEngine(float gravity)
        {
            _gravity = gravity;
            _objects = new List<PhysicsObject>();
            _broadPhaseCollider = new OctreeBroadPhaseCollider(this);
            _narrowPhaseCollider = SATCollider.Instance;
            ActiveCollisions = new CollisionList();
            collisionPool = new Pool<CollisionPair>(50);
        }

        public IBroadPhaseCollider BroadPhaseCollider
        {
            get { return _broadPhaseCollider; }
        }
        public INarrowPhaseCollider NarrowPhaseCollider
        {
            get { return _narrowPhaseCollider; }
        }

        public List<PhysicsObject> ObjectList
        {
            get { return _objects; }
        }

        public void Add(PhysicsObject obj)
        {
            _objects.Add(obj);
            _broadPhaseCollider.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            ActiveCollisions.PrepareForBroadPhase(_objects);
            DoBroadPhaseCollision();
            DoNarrowPhaseCollision();
            ApplyForces(gameTime);
            //ApplyImpulses(gameTime);
            UpdatePositions(gameTime);
            CleanDisposedObjects();
        }

        private void DoBroadPhaseCollision()
        {
            _broadPhaseCollider.Update();
        }

        private void DoNarrowPhaseCollision()
        {
            foreach (CollisionPair pair in ActiveCollisions)
            {
                pair.Collide();
            }
            ActiveCollisions.RemoveInactiveCollisions(collisionPool);
        }

        private void ApplyForces(GameTime gameTime)
        {
            foreach (PhysicsObject obj in _objects)
            {
                if (obj.IsStatic || obj.IsDisposed || !obj.Enabled) continue;
                obj.ApplyImpulses();

                // gravity
                obj._force.Y += _gravity * obj._mass;

                obj.IntegrateVelocity(gameTime);
                obj.ClearForce();
            }
        }

        private void ApplyImpulses(GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds * 0.001f; // in seconds.
            float inverseDt = 1.0f / dt;
            
            /*
            foreach (CollisionPair pair in ActiveCollisions)
            {
                pair.PreStepImpulse(ref inverseDt);
            }
            */
             
            //for (int i = 0; i < Iterations; i++)
            {
                foreach (CollisionPair pair in ActiveCollisions)
                {
                    pair.ApplyImpulse();
                }
            }
        }

        private void UpdatePositions(GameTime gameTime)
        {
            foreach (PhysicsObject obj in _objects)
            {
                if (obj.IsStatic || obj.IsDisposed || !obj.Enabled) continue;
                obj.IntegratePosition(gameTime);
            }
            foreach (CollisionPair pair in ActiveCollisions)
            {
                if (pair.ObjectA.Enabled && !pair.ObjectA.IsStatic)
                {
                    pair.ObjectA.Position += pair.contact.Normal;
                    if (pair.contact.Normal.Y > 0) pair.ObjectA.OnGround = true;
                }
                if (pair.ObjectB.Enabled && !pair.ObjectB.IsStatic)
                {
                    pair.ObjectB.Position -= pair.contact.Normal;
                }
            }
        }

        private void CleanDisposedObjects()
        {
            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                if (_objects[i].IsDisposed)
                    _objects.RemoveAt(i);
            }
        }

    }
}
