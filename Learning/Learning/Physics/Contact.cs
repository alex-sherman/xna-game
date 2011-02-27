using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class Contact
    {
        public Vector3 Normal;
        //public float Separation;

        public Contact(Vector3 normal)
        {
            Normal = normal;
            //Separation = separation;
        }
    }
}
