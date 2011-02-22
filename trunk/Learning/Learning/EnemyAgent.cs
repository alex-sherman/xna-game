using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class EnemyAgent : GameObject
    {
        public static Model model;
        public Vector3 outsideV = Vector3.Zero;
        public Vector3 currentAbsVelocity = Vector3.Zero;
        public Vector3 currentRelVelocity = Vector3.Zero;
        float xRotation = 0, yRotation = 0;
        World world;

        public EnemyAgent(World world)
            : base()
        {
            this.world = world;
            hitBox = CalculateBoundingBox();
            //this.model = model;
        }

        public EnemyAgent(Vector3 position, World world)
            : base(position)
        {
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
            //hitBox.Max = Position + GameConstants.PlayerSize;
            //hitBox.Min = Position - GameConstants.PlayerSize;

            //rotation = Matrix.CreateRotationX(yRotation) * Matrix.CreateRotationY(xRotation);
            currentAbsVelocity = Vector3.Transform(currentRelVelocity, Matrix.CreateRotationY(xRotation));

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
                    effect.World = Matrix.CreateTranslation(Position);// *worldMatrix * transforms[mesh.ParentBone.Index];
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
