using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    public class CollisionIDGenerator
    {
        private Int64 _curID = 0;
        public Int64 generateID()
        {
            return _curID++;
        }
    }
}
