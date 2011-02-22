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

        public EnemyAgent()
            : base()
        {
            hitBox = CalculateBoundingBox();
            //this.model = model;
        }

        public EnemyAgent(Vector3 position)
            : base(position)
        {
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
