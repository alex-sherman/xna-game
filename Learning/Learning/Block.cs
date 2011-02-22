using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace Learning
{
    [Serializable()]
    class Block : GameObject, ISerializable
    {
        public int type = 0;
        public static Texture2D[] textureList;
        public Block(Vector3 position,int type)
            : base(position)
        {
            this.hitBox.Max = 2 * Cube.cubeSize * position + new Vector3(Cube.cubeSize);
            this.hitBox.Min = 2 * Cube.cubeSize * position - new Vector3(Cube.cubeSize);
            Position = 2 * Cube.cubeSize * position;
            this.type = type;
        }
        public Block(SerializationInfo info, StreamingContext context)
        {
            this.hitBox = (BoundingBox)info.GetValue("hitBox", typeof(BoundingBox));
            Position = (Vector3)info.GetValue("position", typeof(Vector3));
            this.type = (int)info.GetValue("type", typeof(int));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("hitBox", hitBox);
            info.AddValue("position", Position);
            info.AddValue("type", this.type);

        }
        public static void initTextures(Texture2D[] textureList)
        {
            Block.textureList = textureList;
        }
        public Boolean canMove(BoundingBox vBox)
        {
            return this.hitBox.Intersects(vBox);
        }
        public override void Draw(World world)
        {
                Cube.Draw(Position, world, this.getTexture());
        }
        public Vector3 getNormal(Ray lookat)
        {
            float? distance = 100;
            BoundingBox[] faces = this.getFaces();
            int closestFace = -1;
            Vector3[] normals = {new Vector3(0,0,1),new Vector3(0,0,-1),new Vector3(1,0,0),
                                 new Vector3(-1,0,0), new Vector3(0,1,0), new Vector3(0,-1,0)};

            for(int i = 0; i<faces.Length;i++)
            {
                if (lookat.Intersects(faces[i])<distance)
                {
                    distance = lookat.Intersects(faces[i]);
                    closestFace = i;
                }
            }
            if (closestFace != -1) { return normals[closestFace]; }
            return new Vector3(0,2,0);
        }
        private BoundingBox[] getFaces()
        {
            BoundingBox front = new BoundingBox();
            BoundingBox back = new BoundingBox();
            BoundingBox right = new BoundingBox();
            BoundingBox left = new BoundingBox();
            BoundingBox top = new BoundingBox();
            BoundingBox bottom = new BoundingBox();

            front.Max = Position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            front.Min = Position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, Cube.cubeSize);

            back.Max = Position + new Vector3(Cube.cubeSize, Cube.cubeSize, -Cube.cubeSize);
            back.Min = Position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            left.Max = Position + new Vector3(-Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            left.Min = Position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            right.Max = Position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            right.Min = Position + new Vector3(Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            top.Max = Position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            top.Min = Position + new Vector3(-Cube.cubeSize, Cube.cubeSize, -Cube.cubeSize);

            bottom.Max = Position + new Vector3(Cube.cubeSize, -Cube.cubeSize, Cube.cubeSize);
            bottom.Min = Position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);
            BoundingBox[] toReturn = { front, back, right, left, top, bottom };
            return toReturn;
        }
        public Texture2D getTexture()
        {
            return Block.textureList[this.type];
        }
        public int[] getDirection(Vector3 other)
        {
            int[] toReturn = { (int)(other.X - Position.X),(int)(other.Y - Position.Y), (int)(other.Z - Position.Z) };
            return toReturn;
        }
    }
}
