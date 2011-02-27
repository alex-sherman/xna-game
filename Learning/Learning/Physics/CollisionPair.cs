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
        public Int64 CollisionID;

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

        internal void Reset()
        {
            ObjectA = null;
            ObjectB = null;
            contact = null;
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
            ObjectB.ApplyImpulse(ref _normalImpulse);

            Vector3.Multiply(ref _normalImpulse, -1.0f, out _normalImpulse);
            ObjectA.ApplyImpulse(ref _normalImpulse);

            if (!Vector3.Zero.Equals(_normalImpulse))
                GUI.print(String.Format("Applying impulse ({0}, {1}, {2}) to {3}", _normalImpulse.X, _normalImpulse.Y, _normalImpulse.Z, ObjectA.ToString()));

        }

        internal void Collide()
        {
            contact = null;
            for (int i = 0; i < _engine.Iterations; i++)
                _engine.NarrowPhaseCollider.Collide(ObjectA, ObjectB, out isColliding, out contact);
        }

        internal bool isInvalid()
        {
            return (ObjectA.IsDisposed || ObjectB.IsDisposed || !isColliding);
        }

        #region IEquatable

        public bool Equals(CollisionPair other)
        {
            return (ObjectA == other.ObjectA) && (ObjectB == other.ObjectB);
        }

        #endregion
    }
}
