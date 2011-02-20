using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Learning
{
    class Block
    {
        public Vector3 position;
        public int type = 0;
        public BoundingBox hitBox = new BoundingBox();
        public static Texture2D[] textureList;
        public Block(Vector3 position,int type)
        {
            this.hitBox.Max = 2 * Cube.cubeSize * position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            this.hitBox.Min = 2 * Cube.cubeSize * position - new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            this.position = 2 * Cube.cubeSize * position;
            this.type = type;
        }
        public static void initTextures(Texture2D[] textureList)
        {
            Block.textureList = textureList;
        }
        public Boolean canMove(BoundingBox vBox)
        {
            return this.hitBox.Intersects(vBox);
        }
        public void Draw(World world)
        {
                Cube.Draw(this.position, world, this.getTexture());
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

            front.Max = this.position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            front.Min = this.position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, Cube.cubeSize);

            back.Max = this.position + new Vector3(Cube.cubeSize, Cube.cubeSize, -Cube.cubeSize);
            back.Min = this.position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            left.Max = this.position + new Vector3(-Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            left.Min = this.position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            right.Max = this.position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            right.Min = this.position + new Vector3(Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);

            top.Max = this.position + new Vector3(Cube.cubeSize, Cube.cubeSize, Cube.cubeSize);
            top.Min = this.position + new Vector3(-Cube.cubeSize, Cube.cubeSize, -Cube.cubeSize);

            bottom.Max = this.position + new Vector3(Cube.cubeSize, -Cube.cubeSize, Cube.cubeSize);
            bottom.Min = this.position + new Vector3(-Cube.cubeSize, -Cube.cubeSize, -Cube.cubeSize);
            BoundingBox[] toReturn = { front, back, right, left, top, bottom };
            return toReturn;
        }
        public Texture2D getTexture()
        {
            return Block.textureList[this.type];
        }
        public int[] getDirection(Vector3 other)
        {
            int[] toReturn = { (int)(other.X - this.position.X),(int)(other.Y - this.position.Y), (int)(other.Z - this.position.Z) };
            return toReturn;
        }
    }
}
