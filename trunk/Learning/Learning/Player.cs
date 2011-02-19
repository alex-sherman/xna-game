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
        public float speed = .1f;
        public Inventory inventory;
        public Vector3 position = new Vector3(0, 7, 0);
        public Vector3 velocity = new Vector3(0, 0, 0);
        public Vector3 outsideV = new Vector3(0, 0, 0);
        public Vector3 toAdd;
        public Matrix rotation;
        public int actionProgress;
        public float xRotation;
        public float yRotation;
        public bool isWalking = false;
        public Ray lookAt = new Ray();
        public World world;
        public Player(){
             this.inventory = new Inventory(this);
        }
        public void Update(){
            this.actionProgress++;
            
            this.hitBox.Max = this.position + new Vector3(1f, 0, 1f);
            this.hitBox.Min = this.position - new Vector3(1f, 2, 1f);

            
            this.rotation = Matrix.CreateRotationX(this.yRotation) * Matrix.CreateRotationY(this.xRotation);
            this.lookAt.Direction = Vector3.Transform(Vector3.UnitZ, this.rotation);
            this.lookAt.Position = this.position;
            this.toAdd = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation));
            toAdd += this.outsideV;


            if (this.toAdd.Z > 0)
            {
                this.vForward.Max = this.position + new Vector3(.3f, 0, this.toAdd.Z + .375f);
                this.vForward.Min = this.position + new Vector3(-.3f, -1.5f, .375f);
            }
            else
            {
                this.vForward.Min = this.position + new Vector3(-.3f, -1.5f, this.toAdd.Z - .375f);
                this.vForward.Max = this.position + new Vector3(.3f, 0, -.375f);
            }
            if (this.toAdd.X > 0)
            {
                this.vLeft.Max = this.position + new Vector3(this.toAdd.X + .375f, 0, .3f);
                this.vLeft.Min = this.position + new Vector3(.375f, -1.5f, -.3f);
            }
            else
            {
                this.vLeft.Min = this.position + new Vector3(this.toAdd.X - .375f, -1.5f, -.3f);
                this.vLeft.Max = this.position + new Vector3(-.375f, 0, .3f);
            }

            if (this.toAdd.Y > 0)
            {

                this.fallBox.Max = this.position + new Vector3(.3f, this.toAdd.Y, .3f);
                this.fallBox.Min = this.position + new Vector3(-.3f, 0, -.3f);
            }
            else
            {
                this.fallBox.Min = this.position + new Vector3(-.3f,this.toAdd.Y - 1.6f, -.3f);
                this.fallBox.Max = this.position + new Vector3(.3f, 0, .3f);
            }

            world.collisionCheck(this);




            this.position += this.toAdd;

        }
        public Matrix getCameraMatrix()
        {
            
            return Matrix.CreateLookAt(this.position+Vector3.Transform(new Vector3(0,0,-.6f),this.rotation),
                   Vector3.Transform(new Vector3(0,0,.5f),this.rotation)+this.position, Vector3.Transform(Vector3.Up,this.rotation));


        }

    }
}
