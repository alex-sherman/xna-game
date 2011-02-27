#region Using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Learning
{
    [Serializable()]
    abstract class GameObject : Physics.PhysicsObject
    {
        #region Fields
        bool _visible = true;
        #endregion Fields

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        #region Constructors and Initialization

        public GameObject(Vector3 position)
        {
            Position = position;
            hitBox = new BoundingBox();
        }

        public GameObject()
        {
            Position = Vector3.Zero;
            hitBox = new BoundingBox();
        }

        #endregion

        public abstract void Draw(World world);
        public abstract void Update(GameTime gameTime);
    }
}
