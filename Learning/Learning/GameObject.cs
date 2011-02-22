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
    abstract class GameObject
    {
        #region Fields
        Vector3 _position;
        // if this isn't public a ton of headaches are created! Why
        // did microsoft decide to make BoundingBox a mutable struct?
        public BoundingBox hitBox;
        bool _visible = true;
        #endregion Fields

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /*
        public BoundingBox HitBox
        {
            get { return hitBox; }
            set
            {
                hitBox = value;
            }
        }
         * */

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion Properties

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
