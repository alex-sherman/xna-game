using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    class CollisionHashSet
    {
        private bool[] _set;
        private int _numObjects = -1;

        public CollisionHashSet(int numObjects, CollisionList list)
        {
            ClearSet(numObjects, list);
        }

        public void ClearSet(int newNumObjects, CollisionList list)
        {
            if (_numObjects >= newNumObjects)
            {
                Array.Clear(_set, 0, _set.Length);
            }
            else
            {
                _numObjects = newNumObjects;
                _set = new bool[CalculateSetSize(newNumObjects)];
            }

            if (list != null)
            {
                foreach (CollisionPair pair in list)
                {
                    pair.ObjectA.CollisionID = CollisionPair.generateCollisionID();
                    pair.ObjectB.CollisionID = CollisionPair.generateCollisionID();
                    TestAndSet(pair.ObjectA, pair.ObjectB);
                }
            }
        }

        public bool TestAndSet(PhysicsObject objA, PhysicsObject objB)
        {
            int index = CalculateIndex(objA, objB);

            bool result = _set[index];
            _set[index] = true;
            return result;
        }

        internal int CalculateIndex(PhysicsObject objA, PhysicsObject objB)
        {
            // first order the objects to ensure unique hashing
            Int64 ID1, ID2;
            if (objA.CollisionID > objB.CollisionID)
            {
                ID1 = objA.CollisionID;
                ID2 = objB.CollisionID;
            }
            else
            {
                ID1 = objB.CollisionID;
                ID2 = objA.CollisionID;
            }

            Int64 result = ID2 * _numObjects;
            if (ID2 % 2 == 0)
            {
                result -= ((ID2 + 1) * (ID2 / 2));
            }
            else
            {
                result -= (ID2 + 1) * (ID2 / 2) + (ID2 + 1) / 2;
            }

            result += ID1 - ID2 - 1;
            return (int)result;
        }

        internal int CalculateSetSize(int numObjects)
        {
            // n choose 2 possible collisions
            return (numObjects * (numObjects - 1)) / 2;
        }
    }
}
