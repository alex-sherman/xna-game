using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    interface INarrowPhaseCollider
    {
        void Collide(PhysicsObject a, PhysicsObject b, out bool Intersect, out Contact contact);
    }
}
