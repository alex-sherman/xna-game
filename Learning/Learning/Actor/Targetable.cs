using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Actor
{
    public struct allegianceStruct
    {
        const int PLAYER = 0;
    }
    interface Targetable
    {
        bool isFriend(Targetable other);
        bool isFoe(Targetable other);
    }
}
