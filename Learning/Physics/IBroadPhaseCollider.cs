using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    delegate bool BroadPhaseCollisionHandler(PhysicsObject obj1, PhysicsObject obj2);

    interface IBroadPhaseCollider
    {
        void Add(PhysicsObject obj);
        void Update();
        event BroadPhaseCollisionHandler OnBroadPhaseCollision;
    }
}
