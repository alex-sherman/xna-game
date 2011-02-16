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
        public BoundingBox vForward = new BoundingBox();
        public BoundingBox vLeft = new BoundingBox();
        public BoundingBox fallBox = new BoundingBox();
        public const float speed = .2f;
        public Vector3 position = new Vector3(0, 7, 0);
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Matrix rotation;
        public Boolean CMR = true;
        public Boolean CMF = true;
        public Boolean CMB = true;
        public Boolean CML = true;
        public Chunk curChunk;
        private int xRotation;
        private int yRotation;
        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            this.hitBox.Max = this.position + new Vector3(2f, 0, 2f);
            this.hitBox.Min = this.position - new Vector3(2f, 4, 2f);

            this.xRotation -= dx;
            this.yRotation -= dy;
            this.rotation = Matrix.CreateRotationX(this.yRotation * .0035f) * Matrix.CreateRotationY(this.xRotation * .0035f);
            
            if (keyboard.IsKeyDown(Keys.W))
            {
                this.velocity.Z = -speed;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                this.velocity.Z = speed;
            }
            else { this.velocity.Z = 0; }
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.velocity.X = speed;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                this.velocity.X = -speed;
            }
            else { this.velocity.X = 0; }

            if (this.velocity.Z > 0)
            {
                this.vForward.Max = this.position + new Vector3(2,0,this.velocity.Z+2);
                this.vForward.Min = this.position + new Vector3(-2,-3,0);
            }
            else if (this.velocity.Z<0)
            {
                this.vForward.Min = this.position + new Vector3(-2, -3, this.velocity.Z-2);
                this.vForward.Max = this.position + new Vector3(2, 0, 0);
            }
            if (this.velocity.X > 0)
            {
                this.vLeft.Max = this.position + new Vector3(this.velocity.X+2, 0, 2);
                this.vLeft.Min = this.position + new Vector3(0,-3.9f,-2);
            }
            else
            {
                this.vLeft.Min = this.position + new Vector3(this.velocity.X-2, -3.9f, -2);
                this.vLeft.Max = this.position + new Vector3(0, 0, 2);
            }
            if (this.velocity.Y > 0)
            {
                this.fallBox.Max = this.position + new Vector3(0, this.velocity.Y, 0);
                this.fallBox.Min = this.position + new Vector3(0,-4,0);
            }
            else
            {
                this.fallBox.Min = this.position + new Vector3(-1, this.velocity.Y-4, -1);
                this.fallBox.Max = this.position + new Vector3(1, 0, 1);
            }
            Boolean[] canMove = curChunk.collisionCheck(this);
            if (canMove[1]) { this.velocity.Y -= .01f; }
            else { this.velocity.Y = 0; }
            if (keyboard.IsKeyDown(Keys.Space) && !canMove[1])
            {
                this.velocity.Y = .21f;
            }
            else if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                this.position -= Vector3.Up * speed;
            }
            if (!canMove[0]) { this.velocity.X = 0; }
            if (!canMove[2]) { this.velocity.Z = 0; }
            Vector3 temp = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation * .0035f));
           
            

            
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
