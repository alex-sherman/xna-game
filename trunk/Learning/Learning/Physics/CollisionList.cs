using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    class CollisionList : List<CollisionPair>
    {
        //private CollisionHashSet _hashSet;

        public CollisionList() : base() { }

        // TODO check that a pair isn't already in the list
        // TODO figure out how to implement efficient hashing. is it necessary?
        public void AddPair(PhysicsObject objA, PhysicsObject objB, PhysicsEngine engine)
        {
            //if (!_hashSet.TestAndSet(objA, objB))
            {
                CollisionPair newPair = engine.collisionPool.Get();
                newPair.ConstructPair(objA, objB, engine);
                Add(newPair);
            }
        }

        public void PrepareForBroadPhase(List<PhysicsObject> objectList)
        {
            CollisionIDGenerator idGen = new CollisionIDGenerator();
            foreach (PhysicsObject obj in objectList)
            {
                obj.CollisionID = idGen.generateID();
            }
            // TODO figure out a way to do this efficiently.
            /*
            if (_hashSet == null)
            {
                _hashSet = new CollisionHashSet(objectList.Count, this);
            }
            else
            {
                _hashSet.ClearSet(objectList.Count, this);
            }
             */
        }


        public void RemoveInactiveCollisions(Pool<CollisionPair> pool)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                CollisionPair pair = this[i];
                if (!this[i].isColliding)
                {
                    RemoveAt(i);
                    pool.Replace(pair);
                }
            }
        }

        public void CleanList(Pool<CollisionPair> pool)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (this[i].isInvalid())
                {
                    CollisionPair current = this[i];
                    RemoveAt(i);
                    pool.Replace(current);
                }
            }
        }
    }
}
