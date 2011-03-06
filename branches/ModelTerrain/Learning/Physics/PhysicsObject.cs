using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning.Physics
{
    class PhysicsObject : IIsDisposable
    {
        private Vector3 _position = Vector3.Zero;
        private float _yRotation;
        private Vector3 _dv = Vector3.Zero; // change in velocity
        private Vector3 _dx = Vector3.Zero; // change in position
        internal float _mass = 1;
        internal float _inverseMass = 1;
        internal Vector3 _force = Vector3.Zero;
        internal Vector3 _impulse = Vector3.Zero;
        internal Vector3 _acceleration = Vector3.Zero;

        public BoundingBox hitBox;
        public Vector3 LinearVelocity = Vector3.Zero;
        public bool IsStatic = false;
        public Int64 CollisionID;
        public bool Enabled = true;
        public bool OnGround = false;

        public event EventHandler<EventArgs> OnDisposed;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float YRotation
        {
            get { return _yRotation; }
            set { _yRotation = value; }
        }

        public BoundingBox AABB
        {
            get { return hitBox; }
            set { hitBox = value; }
        }

        public float Mass
        {
            get { return _mass; }
            set
            {
                if (value > 0)
                    _mass = value;
                if (!IsStatic)
                    _inverseMass = 1.0f / value;
            }
        }

        public Vector3 Force
        {
            get { return _force; }
        }

        public PhysicsObject()
        {
            IsDisposed = false;
        }

        public void ApplyForce(ref Vector3 amount)
        {
            _force.X += amount.X;
            _force.Y += amount.Y;
            _force.Z += amount.Z;
        }

        public void ClearForce()
        {
            _force.X = 0;
            _force.Y = 0;
            _force.Z = 0;
        }

        public void ApplyImpulse(ref Vector3 amount)
        {
            _impulse.X += amount.X;
            _impulse.Y += amount.Y;
            _impulse.Z += amount.Z;
        }

        public void ClearImpulse()
        {
            _impulse.X = 0;
            _impulse.Y = 0;
            _impulse.Z = 0;
        }

        internal void ApplyImpulses()
        {
            ApplyImpulseNow(ref _impulse);
            ClearImpulse();
        }

        internal void ApplyImpulseNow(ref Vector3 impulse)
        {
            // TODO check performance of inlining these
            Vector3.Multiply(ref impulse, _inverseMass, out _dv);

            Vector3.Add(ref LinearVelocity, ref _dv, out LinearVelocity);
        }

        internal void IntegrateVelocity(GameTime gameTime)
        {
            Vector3.Multiply(ref _force, _inverseMass, out _acceleration);
            Vector3.Multiply(ref _acceleration,
                             gameTime.ElapsedGameTime.Milliseconds * 0.001f,
                             out _dv);
            Vector3.Add(ref LinearVelocity, ref _dv, out LinearVelocity);
        }

        internal void IntegratePosition(GameTime gameTime)
        {
            Vector3.Multiply(ref LinearVelocity,
                             gameTime.ElapsedGameTime.Milliseconds * 0.001f,
                             out _dx);
            Vector3.Add(ref _dx, ref _position, out _position);
        }

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            if (OnDisposed != null)
            {
                OnDisposed(this, EventArgs.Empty);
            }
        }
    }
}
