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
        public Inventory inventory;
        public Vector3 position = new Vector3(0, 7, 0);
        private Vector3 velocity = new Vector3(0, 0, 0);
        private Vector3 outsideV = new Vector3(0, 0, 0);
        private Vector3 toAdd;
        private Matrix rotation;
        public int actionProgress;
        private float xRotation;
        private float yRotation;
        public Ray lookAt = new Ray();
        public World world;
        public Player(){
             this.inventory = new Inventory(this);
        }
        public void Update(GameTime time, int dx, int dy, KeyboardState keyboard){
            this.actionProgress++;
            if(keyboard.IsKeyDown(Keys.D1)){
                this.inventory.currentItem = 0;
            }
            if (keyboard.IsKeyDown(Keys.D2))
            {
                this.inventory.currentItem = 1;
            }
            if (keyboard.IsKeyDown(Keys.D3))
            {
                this.inventory.currentItem = 2;
            }
            if (keyboard.IsKeyDown(Keys.D4))
            {
                this.inventory.currentItem = 3;
            }
            if (keyboard.IsKeyDown(Keys.D5))
            {
                this.inventory.currentItem = 4;
            }
            if(keyboard.IsKeyDown(Keys.Up)){
                dy =5;
            }
            if(keyboard.IsKeyDown(Keys.Down)){
                dy =-5;
            }
            if(keyboard.IsKeyDown(Keys.Left)){
                dx =-5;
            }
            if(keyboard.IsKeyDown(Keys.Right)){
                dx = 5;
            }
            this.hitBox.Max = this.position + new Vector3(1f, 0, 1f);
            this.hitBox.Min = this.position - new Vector3(1f, 2, 1f);

            this.xRotation -= dx*.0035f;
            if (this.yRotation > -MathHelper.PiOver2 && dy<0 || this.yRotation < MathHelper.PiOver2 && dy>0)
            {
                this.yRotation += dy * .0035f;
            }
            if (this.xRotation >= 2 * MathHelper.Pi)
            {
                this.xRotation -= 2 * MathHelper.Pi;
            }
            this.rotation = Matrix.CreateRotationX(this.yRotation) * Matrix.CreateRotationY(this.xRotation);
            this.lookAt.Direction = Vector3.Transform(Vector3.UnitZ, this.rotation);
            this.lookAt.Position = this.position;
            if(keyboard.IsKeyDown(Keys.F) && this.actionProgress>=0){
                this.actionProgress = -20;
                this.world.destroyBlock(this.lookAt);
            }
            if (keyboard.IsKeyDown(Keys.Q) && this.actionProgress>=0)
            {
                this.actionProgress = -20;
                Item item = this.inventory.getItem();
                if (item != null)
                {
                    if (this.world.addBlock(this.lookAt, item.type))
                    {
                        this.inventory.useItem();
                    }
                }
            }
            if(keyboard.IsKeyDown(Keys.LeftShift)){
                speed = .05f;
            }
            else{
                speed = .1f;
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                this.velocity.Z = speed;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                this.velocity.Z = -speed;
            }
            else { this.velocity.Z = 0; }
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.velocity.X = -speed;
            }
            else if (keyboard.IsKeyDown(Keys.A))
            {
                this.velocity.X = speed;
            }
            else { this.velocity.X = 0; }
            this.toAdd = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation));
            toAdd += this.outsideV;
            if (this.toAdd.Z > 0)
            {
                this.vForward.Max = this.position + new Vector3(.375f, 0, this.toAdd.Z + .375f);
                this.vForward.Min = this.position + new Vector3(-.375f, -1.5f, 0);
            }
            else
            {
                this.vForward.Min = this.position + new Vector3(-.375f, -1.5f, this.toAdd.Z - .375f);
                this.vForward.Max = this.position + new Vector3(.375f, 0, 0);
            }
            if (this.toAdd.X > 0)
            {
                this.vLeft.Max = this.position + new Vector3(this.toAdd.X + .375f, 0, .375f);
                this.vLeft.Min = this.position + new Vector3(0, -1.5f, -.375f);
            }
            else
            {
                this.vLeft.Min = this.position + new Vector3(this.toAdd.X - .375f, -1.5f, -.375f);
                this.vLeft.Max = this.position + new Vector3(0, 0, .375f);
            }
            if (this.toAdd.Y > 0)
            {
                this.fallBox.Max = this.position + new Vector3(.375f, this.toAdd.Y, .375f);
                this.fallBox.Min = this.position + new Vector3(-.375f, -1.5f, -.375f);
            }
            else
            {
                this.fallBox.Min = this.position + new Vector3(-.375f, this.toAdd.Y - 1.5f, -.375f);
                this.fallBox.Max = this.position + new Vector3(.375f, 0, .375f);
            }
            Boolean[] canMove = world.collisionCheck(this);
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
            
            return Matrix.CreateLookAt(this.position+Vector3.Transform(new Vector3(0,0,-.8f),this.rotation),
                   Vector3.Transform(new Vector3(0,0,.5f),this.rotation)+this.position, Vector3.Transform(Vector3.Up,this.rotation));


        }

    }
}
