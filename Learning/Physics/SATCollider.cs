using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class SATCollider : INarrowPhaseCollider
    {
        private static SATCollider _instance;
        private SATCollider() { }

        public static SATCollider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SATCollider();
                return _instance;
            }
        }

        public void Collide(PhysicsObject a, PhysicsObject b, out bool Intersect, out Contact contact)
        {
            Vector3 penetration = Vector3.Zero;
            if (!a.AABB.Intersects(b.AABB))
                Intersect = false;
            else
            {
                penetration = getMinimumPenetrationVector(a.AABB, b.AABB);

                //if (penetration.Length() > 0.00001f)
                    Intersect = true;
                //else
                //    Intersect = false;
            }

            // correct positions
            if (!a.IsStatic && Intersect)
            {
                //a.Position += penetration;
                a.LinearVelocity.Y = 0;
            }
            if (!b.IsStatic && Intersect)
            {
                //b.Position -= penetration;
                b.LinearVelocity.Y = 0f;
            }
            contact = Intersect ? new Contact(penetration) : null;
        }

        private Vector3 getMinimumPenetrationVector(BoundingBox box1, BoundingBox box2)
        {
            Vector3 result = Vector3.Zero;

            float diff, minDiff;
            int axis, side;

            // neg X
            diff = box1.Max.X - box2.Min.X;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            minDiff = diff;
            axis = 0;
            side = -1;

            // pos X
            diff = box2.Max.X - box1.Min.X;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                side = 1;
            }

            // neg Y
            diff = box1.Max.Y - box2.Min.Y;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 1;
                side = -1;
            }

            // pos Y
            diff = box2.Max.Y - box1.Min.Y;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 1;
                side = 1;
            }

            // neg Z
            diff = box1.Max.Z - box2.Min.Z;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 2;
                side = -1;
            }

            // pos Z
            diff = box2.Max.Z - box1.Min.Z;
            if (diff < 0.0f)
            {
                return Vector3.Zero;
            }
            if (diff < minDiff)
            {
                minDiff = diff;
                axis = 2;
                side = 1;
            }

            // Intersection occurred
            if (axis == 0)
                result.X = (float)side * minDiff;
            else if (axis == 1)
                result.Y = (float)side * minDiff;
            else
                result.Z = (float)side * minDiff;

            return result;
        }
    }
}
