using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Learning
{
    class Player
    {
        public BoundingBox hitBox = new BoundingBox();
        public BoundingBox fallBox = new BoundingBox();
        public const float speed = .2f;
        public Vector3 position = new Vector3(0, 5, 0);
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Matrix rotation;
        public Boolean CMR = true;
        public Boolean CMF = true;
        public Boolean CMB = true;
        public Boolean CML = true;
        private int xRotation;
        private int yRotation;
        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            this.hitBox.Max = this.position + new Vector3(2f, 0, 2f);
            this.hitBox.Min = this.position - new Vector3(2f, 2, 2f);
            this.fallBox.Max = this.position + new Vector3(0, 2, 0);
            this.fallBox.Min = this.position + new Vector3(0, 0, 0);
            this.xRotation -= dx;
            this.yRotation -= dy;
            this.rotation = Matrix.CreateRotationX(this.yRotation * .0035f) * Matrix.CreateRotationY(this.xRotation * .0035f);
            Boolean[] canMove = Chunk.collisionCheck(this);
            this.velocity = Vector3.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                this.velocity.Z = -1;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                this.velocity.Z = 1;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.velocity.X = 1;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                this.velocity.X = -1;
            }
            this.velocity *= speed;

            Vector3 temp = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation * .0035f));
            if (!canMove[0] && temp.X < 0) { temp.X = 0; }
            if (!canMove[1] && temp.X > 0) { temp.X = 0; }
            if (!canMove[2] && temp.Z < 0) { temp.Z = 0; }
            if (!canMove[3] && temp.Z > 0) { temp.Z = 0; }
            if (canMove[4]) { temp.Y -= .05f; }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                temp = Vector3.Up * speed;
            }
            else if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                this.position -= Vector3.Up * speed;
            }
            this.position += temp;

        }
        public Matrix getCameraMatrix()
        {
            
            return Matrix.CreateLookAt(this.position,
                   Vector3.Transform(new Vector3(0,0,-1),this.rotation)+this.position, Vector3.Transform(Vector3.Up,this.rotation));


        }
        public Vector3 getCameraPos()
        {
            return new Vector3(this.position.X,this.position.Y,this.position.Z);
        }

    }
}
