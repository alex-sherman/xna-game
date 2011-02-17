using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning
{
    class Block
    {
        public Vector3 position;
        public int type = 0;
        public BoundingBox hitBox = new BoundingBox();
        public Block(Vector3 position)
        {
            this.hitBox.Max = 2 * Cube.cubeSize * position + new Vector3(1f, 1f, 1f);
            this.hitBox.Min = 2 * Cube.cubeSize * position - new Vector3(1f, 1f, 1f);
            this.position = 2 * Cube.cubeSize * position;
        }
        public Boolean canMove(BoundingBox vBox)
        {
            return this.hitBox.Intersects(vBox);
        }
        public int[] getDirection(Vector3 other)
        {
            int[] toReturn = { (int)(other.X - this.position.X),(int)(other.Y - this.position.Y), (int)(other.Z - this.position.Z) };
            return toReturn;
        }
    }
}
