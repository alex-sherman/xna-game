using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class CollisionPair : IEquatable<CollisionPair>
    {
        private PhysicsEngine _engine;

        public PhysicsObject ObjectA;
        public PhysicsObject ObjectB;
        public Contact contact;

        public bool isColliding;

        public CollisionPair()
        {
            isColliding = true;
        }

        internal void ConstructPair(PhysicsObject objA, PhysicsObject objB, PhysicsEngine engine)
        {
            _engine = engine;
            ObjectA = objA;
            ObjectB = objB;
        }

        private Vector3 _dv, _normalImpulse;
        private Vector3 _normal;
        private float _vn;

        internal void ApplyImpulse()
        {
            // calculate velocity difference
            Vector3.Subtract(ref ObjectA.LinearVelocity, ref ObjectB.LinearVelocity, out _dv);
            _normal = contact.Normal;
            _normal.Normalize();
            Vector3.Dot(ref _dv, ref _normal, out _vn);
            Vector3.Multiply(ref _normal, _vn, out _normalImpulse);
            ObjectA.ApplyImpulse(ref _normalImpulse);

            Vector3.Multiply(ref _normalImpulse, -1.0f, out _normalImpulse);
            ObjectB.ApplyImpulse(ref _normalImpulse);
        }

        internal void Collide()
        {
            contact = null;
            _engine.NarrowPhaseCollider.Collide(ObjectA, ObjectB, out isColliding, out contact);
        }

        #region IEquatable

        public bool Equals(CollisionPair other)
        {
            return (ObjectA == other.ObjectA) && (ObjectB == other.ObjectB);
        }

        #endregion
    }
}
