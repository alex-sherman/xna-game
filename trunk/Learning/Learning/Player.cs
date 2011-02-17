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
        private float speed = .1f;
        public Vector3 position = new Vector3(0, 7, 3);
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Vector3 outsideV = new Vector3(0, 0, 0);
        private Vector3 toAdd;
        private Matrix rotation;
        public Chunk curChunk;
        private float xRotation;
        private float yRotation;

        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            if(keyboard.IsKeyDown(Keys.Up)){
                dy =-5;
            }
            if(keyboard.IsKeyDown(Keys.Down)){
                dy =5;
            }
            if(keyboard.IsKeyDown(Keys.Left)){
                dx =-5;
            }
            if(keyboard.IsKeyDown(Keys.Right)){
                dx = 5;
            }
            this.curChunk = Chunk.getChunk(this.position);
            this.hitBox.Max = this.position + new Vector3(2f, 0, 2f);
            this.hitBox.Min = this.position - new Vector3(2f, 4, 2f);

            this.xRotation -= dx*.0035f;
            if (this.yRotation > -MathHelper.PiOver2 && dy>0 || this.yRotation < MathHelper.PiOver2 && dy<0)
            {
                this.yRotation -= dy * .0035f;
            }
            if (this.xRotation >= 2 * MathHelper.Pi)
            {
                this.xRotation -= 2 * MathHelper.Pi;
            }
            this.rotation = Matrix.CreateRotationX(this.yRotation) * Matrix.CreateRotationY(this.xRotation);
            if(keyboard.IsKeyDown(Keys.LeftShift)){
                speed = .2f;
            }
            else{
                speed = .1f;
            }
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
                this.vForward.Max = this.position + new Vector3(.75f, 0, this.toAdd.Z + .75f);
                this.vForward.Min = this.position + new Vector3(-.75f, -3, 0);
            }
            else
            {
                this.vForward.Min = this.position + new Vector3(-.75f, -3, this.toAdd.Z - .75f);
                this.vForward.Max = this.position + new Vector3(.75f, 0, 0);
            }
            if (this.toAdd.X > 0)
            {
                this.vLeft.Max = this.position + new Vector3(this.toAdd.X + .75f, 0, .75f);
                this.vLeft.Min = this.position + new Vector3(0, -3, -.75f);
            }
            else
            {
                this.vLeft.Min = this.position + new Vector3(this.toAdd.X - .75f, -3, -.75f);
                this.vLeft.Max = this.position + new Vector3(0, 0, .75f);
            }
            if (this.toAdd.Y > 0)
            {
                this.fallBox.Max = this.position + new Vector3(0, this.toAdd.Y, 0);
                this.fallBox.Min = this.position + new Vector3(0,-4,0);
            }
            else
            {
                this.fallBox.Min = this.position + new Vector3(-.75f, this.toAdd.Y - 4, -.75f);
                this.fallBox.Max = this.position + new Vector3(.75f, 0, .75f);
            }
            Boolean[] canMove = curChunk.collisionCheck(this);
            if (canMove[1]) { this.outsideV.Y -= .025f; }
            else { this.outsideV.Y = 0; }

            if (!canMove[0]) { this.toAdd.X = 0;  this.outsideV.X = 0; }
            if (!canMove[2]) { this.toAdd.Z = 0;  this.outsideV.Z = 0; }
            if (!canMove[1]) { this.toAdd.Y = 0; this.outsideV.Y = 0; }
            if (keyboard.IsKeyDown(Keys.Space) && !canMove[1])
            {
                this.outsideV.Y = .31f;
            }
            

            
            this.position += this.toAdd;

        }
        public Matrix getCameraMatrix()
        {
            
            return Matrix.CreateLookAt(this.position+Vector3.Transform(new Vector3(0,-.5f,.8f),this.rotation),
                   Vector3.Transform(new Vector3(0,0,-1),this.rotation)+this.position, Vector3.Transform(Vector3.Up,this.rotation));


        }
        public Vector3 getCameraPos()
        {
            return new Vector3(this.position.X,this.position.Y,this.position.Z);
        }

    }
}
