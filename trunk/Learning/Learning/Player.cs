using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Learning
{
    class Player : Physics.PhysicsObject
    {
        #region Declarations
        public float speed = .1f;
        public Inventory inventory;
        public Vector3 relativeVelocity = new Vector3(0, 0, 0);
        public Vector3 outsideV = new Vector3(0, 0, 0);
        public Vector3 currentVelocity;
        public Matrix rotation;
        public bool noClip = true;
        public float xRotation;
        public float yRotation;
        public bool isWalking = false;
        public Ray lookAt = new Ray();
        public World world;

        //OctreeNode containingNode, lastNode;
        //List<GameObject> curCollisionCandidates;

        #endregion

        public Player()
        {
            IsStatic = false;
            Enabled = false;
            this.inventory = new Inventory();
            Position = new Vector3(0, 25, 0);
        }

        public void Update(GameTime gameTime)
        {
            hitBox.Max = Position + GameConstants.PlayerSize;
            hitBox.Min = Position - GameConstants.PlayerSize;
            // now account for the higher location of the camera
            //hitBox.Max.Y -= GameConstants.PlayerSize.Y / 3;
            hitBox.Min.Y -= GameConstants.PlayerSize.Y / 3;

            rotation = Matrix.CreateRotationX(yRotation) * Matrix.CreateRotationY(xRotation);
            lookAt.Direction = Vector3.Transform(Vector3.UnitZ, rotation);
            lookAt.Position = Position;
            //currentVelocity = Vector3.Transform(relativeVelocity, Matrix.CreateRotationY(xRotation));
            currentVelocity = Vector3.Transform(relativeVelocity, rotation);
            Vector3.Add(ref currentVelocity, ref LinearVelocity, out LinearVelocity);
            /*
            outsideV.Y -= GameConstants.Gravity;
            //currentVelocity += outsideV;

            Vector3 endPos = Position;
            endPos += currentVelocity * gameTime.ElapsedGameTime.Milliseconds;
            if (!noClip)
            {
                BoundingBox endAABB = new BoundingBox(
                    endPos - GameConstants.PlayerSize / 2,
                    endPos + GameConstants.PlayerSize / 2);
                // move the player's camera up
                endAABB.Min.Y -= GameConstants.PlayerSize.Y / 3;
                endAABB.Max.Y -= GameConstants.PlayerSize.Y / 3;
                curCollisionCandidates = world.objectTree.getCollisionCandidates(endAABB);


                // resolve non-gravity-caused collisions

                //endPos += outsideV * gameTime.ElapsedGameTime.Milliseconds;

                int checkedCollisions = 0;
                while (world.collisionCheck(curCollisionCandidates, ref endPos, ref isWalking, ref outsideV) && ++checkedCollisions < 4)
                    if (isWalking) outsideV.Y = 0;
                //world.collisionCheck(curCollisionCandidates, ref endAABB, ref endPos, ref isWalking, ref outsideV);

                //gravity (to get the true state of isWalking)
                endPos += outsideV * gameTime.ElapsedGameTime.Milliseconds;
                isWalking = false;
                world.collisionCheck(curCollisionCandidates, ref endPos, ref isWalking, ref outsideV);
            }
            else
            {
                outsideV = Vector3.Zero;
            }
             */
            // and update the player's position
            // if not enabled, update positions manually
            if (!Enabled)
            {
                Position = Position + currentVelocity * gameTime.ElapsedGameTime.Milliseconds;
            }
            
        }

        public Matrix getCameraMatrix()
        {
            return Matrix.CreateLookAt(
                   Position + Vector3.Transform(new Vector3(0, 0f, -.4f), this.rotation),
                   Vector3.Transform(new Vector3(0, 0, .5f), this.rotation) + this.Position,
                   Vector3.Transform(Vector3.Up, this.rotation));
        }

    }
}
