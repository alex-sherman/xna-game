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
        public Vector3 currentVelocity;
        public Matrix rotation;
        public int actionProgress;
        public float xRotation;
        public float yRotation;
        public bool isWalking = false;
        public Ray lookAt = new Ray();
        public World world;

        public Player()
        {
            this.inventory = new Inventory(this);
        }

        public void Update(GameTime gameTime)
        {
            this.actionProgress++;

            this.hitBox.Max = this.position + new Vector3(1f, 0, 1f);
            this.hitBox.Min = this.position - new Vector3(1f, 2, 1f);


            this.rotation = Matrix.CreateRotationX(this.yRotation) * Matrix.CreateRotationY(this.xRotation);
            this.lookAt.Direction = Vector3.Transform(Vector3.UnitZ, this.rotation);
            this.lookAt.Position = this.position;
            this.currentVelocity = Vector3.Transform(this.velocity, Matrix.CreateRotationY(this.xRotation));

            outsideV.Y -= GameConstants.Gravity;
            currentVelocity += this.outsideV;
            
            Vector3 endPos = position;
            endPos += currentVelocity * gameTime.ElapsedGameTime.Milliseconds;

            // resolve a few possible collisions and check if on ground
            // at most 5 are possible
            isWalking = false;
            for (int i = 0; i < 5; i++)
                world.collisionCheck(ref endPos, ref isWalking);

            if (isWalking)
                outsideV.Y = 0;

            // and update the player's position
            position = endPos; 
        }

        public Matrix getCameraMatrix()
        {

            return Matrix.CreateLookAt(
                   position + Vector3.Transform(new Vector3(0, 0f, -.4f), this.rotation),
                   Vector3.Transform(new Vector3(0, 0, .5f), this.rotation) + this.position, 
                   Vector3.Transform(Vector3.Up, this.rotation));


        }

    }
}
