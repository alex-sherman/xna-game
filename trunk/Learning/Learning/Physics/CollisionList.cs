using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    class CollisionList : List<CollisionPair>
    {

        public CollisionList() : base() { }

        // TODO check that a pair isn't already in the list
        public void AddPair(PhysicsObject objA, PhysicsObject objB, PhysicsEngine engine)
        {
            CollisionPair newPair = engine.collisionPool.Get();
            newPair.ConstructPair(objA, objB, engine);
            Add(newPair);
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
