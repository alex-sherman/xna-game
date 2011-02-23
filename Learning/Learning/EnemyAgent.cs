using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    enum AgentState
    {
        Following
    }

    class EnemyAgent : GameObject
    {
        public static Model model;
        public Vector3 outsideV = Vector3.Zero;
        public Vector3 currentAbsVelocity = Vector3.Zero;
        public Vector3 currentRelVelocity = Vector3.Zero;
        float speed = 0.001f;
        float xRotation = 0, yRotation = 0;
        Vector3 heading = Vector3.UnitX;
        Matrix rotation = Matrix.Identity;
        World world;
        AgentState state = AgentState.Following;
        AIManager aiManager;

        public EnemyAgent(World world, AIManager manager)
            : base()
        {
            this.aiManager = manager;
            this.world = world;
            hitBox = CalculateBoundingBox();
            //this.model = model;
        }

        public EnemyAgent(Vector3 position, World world, AIManager manager)
            : base(position)
        {
            this.aiManager = manager;
            this.world = world;
            hitBox = CalculateBoundingBox();
            //this.model = model;
        }
        protected BoundingBox CalculateBoundingBox()
        {
            BoundingBox mergedBox = new BoundingBox();
            BoundingBox[] boundingBoxes;
            int index = 0;
            int meshCount = model.Meshes.Count;

            boundingBoxes = new BoundingBox[meshCount];
            foreach (ModelMesh mesh in model.Meshes)
            {
                boundingBoxes[index++] = BoundingBox.CreateFromSphere(mesh.BoundingSphere);
            }

            mergedBox = boundingBoxes[0];
            if ((model.Meshes.Count) > 1)
            {
                index = 1;
                do
                {
                    mergedBox = BoundingBox.CreateMerged(mergedBox,
                        boundingBoxes[index]);

                    index++;
                } while (index < model.Meshes.Count);
            }
            //mergedBox.Center.Y = 0;
            return mergedBox;
        }

        public override void Update(GameTime gameTime)
        {
            float turnSpeed = 0.1f;
            Vector3 desiredHeading = aiManager.getHeading(this, world.players[0].position);
            Vector3 headingDifference = desiredHeading - heading;
            headingDifference.Normalize();
            heading += headingDifference * turnSpeed;
            heading.Normalize();

            yRotation = (float)Math.Atan2(heading.Z, heading.X);
            //yRotation = 0; //??

            rotation = Matrix.CreateRotationY(yRotation);
            //currentAbsVelocity = Vector3.Transform(currentRelVelocity, Matrix.CreateRotationY(xRotation));
            currentAbsVelocity = heading * speed;
            
            outsideV.Y -= GameConstants.Gravity;
            //currentVelocity += outsideV;

            Vector3 endPos = Position;
            endPos += currentAbsVelocity * gameTime.ElapsedGameTime.Milliseconds;

            // resolve non-gravity-caused collisions
            bool onGround = false;
            for (int i = 0; i < 8; i++)
                world.collisionCheck(ref endPos, ref onGround, ref outsideV);

            // gravity (to get the true state of isWalking)
            //isWalking = false;
            endPos += outsideV * gameTime.ElapsedGameTime.Milliseconds;

            world.collisionCheck(ref endPos, ref onGround, ref outsideV);

            Vector3 amountMoved = endPos - Position;
            hitBox.Max += amountMoved;
            hitBox.Min += amountMoved;

            // and update the position
            Position = endPos;
        }

        public override void Draw(World world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix worldMatrix = world.partialWorld;
            World.device.RasterizerState = RasterizerState.CullCounterClockwise;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(Position) * transforms[mesh.ParentBone.Index];// *rotation;
                    effect.View = world.partialWorld;
                    effect.Projection = world.projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }
        }
    }
}
