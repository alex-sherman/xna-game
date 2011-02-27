using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class PhysicsEngine
    {
        private List<PhysicsObject> _objects;
        private IBroadPhaseCollider _broadPhaseCollider;
        private INarrowPhaseCollider _narrowPhaseCollider;
        private Pool<CollisionPair> collisionPool;
        private float _gravity;

        public CollisionList ActiveCollisions;

        public int Iterations = 5;

        public PhysicsEngine(float gravity)
        {
            _gravity = gravity;
            _objects = new List<PhysicsObject>();
            _broadPhaseCollider = new OctreeBroadPhaseCollider(this);
            _narrowPhaseCollider = SAT.Instance;
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
            _broadPhaseCollider.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            DoBroadPhaseCollision();
            DoNarrowPhaseCollision();
            ApplyForces(gameTime);
            ApplyImpulses(gameTime);
            UpdatePositions(gameTime);
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
                if (obj.IsStatic) continue;
                obj.ApplyImpulses();

                // gravity
                obj._force.Y += _gravity * obj._mass;

                obj.IntegrateVelocity(gameTime);
                obj.ClearForce();
            }
        }

        private void ApplyImpulses(GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds;
            float inverseDt = 1.0f / dt;
            /*
            foreach (CollisionPair pair in ActiveCollisions)
            {
                pair.PreStepImpulse(ref inverseDt);
            }

             */
            for (int i = 0; i < Iterations; i++)
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
                obj.IntegratePosition(gameTime);
            }
        }

    }
}
