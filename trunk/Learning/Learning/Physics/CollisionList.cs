using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    class CollisionList : List<CollisionPair>
    {

        public CollisionList() : base() { }

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
