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
        private Vector3 outsideV = new Vector3(0, 0, 0);
        private Vector3 toAdd;
        private Matrix rotation;
        public Chunk curChunk;
        private float xRotation;
        private float yRotation;
        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            this.hitBox.Max = this.position + new Vector3(2f, 0, 2f);
            this.hitBox.Min = this.position - new Vector3(2f, 4, 2f);

            this.xRotation -= dx*.0035f;
            this.yRotation -= dy * .0035f;
            this.rotation = Matrix.CreateRotationX(this.yRotation) * Matrix.CreateRotationY(this.xRotation);
            
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
            this.toAdd = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation));
            toAdd += this.outsideV;
            if (this.toAdd.Z > 0)
            {
                this.vForward.Max = this.position + new Vector3(2, 0, this.toAdd.Z + 2);
                this.vForward.Min = this.position + new Vector3(-2,-3,0);
            }
            else
            {
                this.vForward.Min = this.position + new Vector3(-2, -3, this.toAdd.Z - 2);
                this.vForward.Max = this.position + new Vector3(2, 0, 0);
            }
            if (this.toAdd.X > 0)
            {
                this.vLeft.Max = this.position + new Vector3(this.toAdd.X + 2, 0, 2);
                this.vLeft.Min = this.position + new Vector3(0,-3.9f,-2);
            }
            else
            {
                this.vLeft.Min = this.position + new Vector3(this.toAdd.X - 2, -3, -2);
                this.vLeft.Max = this.position + new Vector3(0, 0, 2);
            }
            if (this.toAdd.Y > 0)
            {
                this.fallBox.Max = this.position + new Vector3(0, this.toAdd.Y, 0);
                this.fallBox.Min = this.position + new Vector3(0,-4,0);
            }
            else
            {
                this.fallBox.Min = this.position + new Vector3(-2, this.toAdd.Y - 4, -2);
                this.fallBox.Max = this.position + new Vector3(2, 0, 2);
            }
            Boolean[] canMove = curChunk.collisionCheck(this);
            if (canMove[1]) { this.outsideV.Y -= .01f; }
            else { this.outsideV.Y = 0; }

            if (!canMove[0]) { this.toAdd.X = 0;  this.outsideV.X = 0; }
            if (!canMove[2]) { this.toAdd.Z = 0;  this.outsideV.Z = 0; }
            if (!canMove[1]) { this.velocity.Y = 0; this.outsideV.Y = 0; }
            if (keyboard.IsKeyDown(Keys.Space) && !canMove[1])
            {
                this.outsideV.Y = .21f;
            }
            

            
            this.position += this.toAdd;

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
