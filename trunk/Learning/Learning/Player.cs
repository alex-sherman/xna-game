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
        #region Declarations
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

        #endregion

        public Player()
        {
            this.inventory = new Inventory();
        }

        public void Update(GameTime gameTime)
        {
            actionProgress++;

            hitBox.Max = position + GameConstants.PlayerSize;
            hitBox.Min = position - GameConstants.PlayerSize;
            // now account for the higher location of the camera
            //hitBox.Max.Y -= GameConstants.PlayerSize.Y / 3;
            hitBox.Min.Y -= GameConstants.PlayerSize.Y / 3;


            rotation = Matrix.CreateRotationX(yRotation) * Matrix.CreateRotationY(xRotation);
            lookAt.Direction = Vector3.Transform(Vector3.UnitZ, rotation);
            lookAt.Position = position;
            currentVelocity = Vector3.Transform(velocity, Matrix.CreateRotationY(xRotation));

            outsideV.Y -= GameConstants.Gravity;
            //currentVelocity += outsideV;
            
            Vector3 endPos = position;
            endPos += currentVelocity * gameTime.ElapsedGameTime.Milliseconds;

            // resolve non-gravity-caused collisions
            for (int i = 0; i < 8; i++)
                world.collisionCheck(ref endPos, ref isWalking, ref outsideV);

            // gravity (to get the true state of isWalking)
            isWalking = false;
            endPos += outsideV * gameTime.ElapsedGameTime.Milliseconds;
            world.collisionCheck(ref endPos, ref isWalking, ref outsideV);

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
