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
            this.hitBox.Max = position + new Vector3(.5f, .5f, .5f);
            this.hitBox.Min = position - new Vector3(.5f, .5f, .5f);
            this.position = position;
        }
        public Boolean checkHit(Player player)
        {
            return this.hitBox.Intersects(player.hitBox);
        }
        public int[] getDirection(Vector3 other)
        {
            int[] toReturn = { (int)(other.X - this.position.X),(int)(other.Y - this.position.Y), (int)(other.Z - this.position.Z) };
            return toReturn;
        }
    }
}
