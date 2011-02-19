using System;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Learning
{
    class Chunk
    {
        public Vector3 position;
        private Block[, ,] BlockList;
        private World world;
        public BoundingBox hitBox;
        public Chunk(Vector3 position, World world)
        {
            this.world = world;
            this.position = position;
            this.hitBox.Min = position;
            this.hitBox.Max = position + new Vector3(10, 10, 10);
            this.addFloor();
        }

        public void addBlock(int x, int y, int z, int type)
        {
            Block poo = new Block(new Vector3(x + this.position.X, y + this.position.Y, z + this.position.Z), type);
            this.BlockList[x, y, z] = poo;
        }
        public void addBlock(Vector3 position, int type)
        {
            Block poo = new Block(position + this.position, type);
            this.BlockList[(int)position.X, (int)position.Y, (int)position.Z] = poo;
        }
        public Block getBlock(Ray lookat)
        {
            Block closestBlock = null;
            float? closestDistance = 100;
            float? distance;
            foreach (Block block in this.BlockList)
            {
                if (block != null)
                {
                    distance = lookat.Intersects(block.hitBox);
                    if (distance != null && distance < closestDistance && distance <= 5)
                    {
                        closestBlock = block;
                        closestDistance = distance;
                    }
                }
            }
            return closestBlock;
        }
        public bool destroyBlock(Block toDestroy)
        {
            if (toDestroy == null) { return false; }
            Vector3 relation = toDestroy.position - this.position;
            int i = (int)(relation.X);
            int j = (int)(relation.Y);
            int k = (int)(relation.Z);
            try
            {
                if (this.BlockList[i, j, k] == toDestroy)
                {
                    this.BlockList[i, j, k] = null;
                    return true;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return false;
        }
        public bool destroyBlock(int i, int j, int k)
        {
            if (this.BlockList[i, j, k] != null)
            {
                this.BlockList[i, j, k] = null;
                return true;
            }


            return false;
        }

        public float[] collisionCheck(Player player)
        {
            Vector3 otherVec = player.position;
            float[] toReturn = { 0, 0, 0 };
            foreach (Block block in BlockList)
            {
                if (block != null)
                {
                    if (block.canMove(player.vLeft))
                    {
                        toReturn[0] = block.position.X - player.position.X;
                        if (toReturn[0] < 0)
                        {
                            toReturn[0] += Cube.cubeSize + .357f;
                        }
                        if (toReturn[0] > 0)
                        {
                            toReturn[0] -= Cube.cubeSize + .357f;
                        }
                    }
                    if (block.canMove(player.fallBox))
                    {
                        toReturn[1] = block.position.Y - player.position.Y;
                        if (toReturn[1] < 0)
                        {
                            toReturn[1] += Cube.cubeSize + 1.5f;
                        }
                        if (toReturn[1] > 0)
                        {
                            toReturn[1] -= Cube.cubeSize + 1.5f;
                        }
                    }
                    if (block.canMove(player.vForward))
                    {
                        toReturn[2] = block.position.Z - player.position.Z;
                        if (toReturn[2] < 0)
                        {
                            toReturn[2] += Cube.cubeSize + .357f;
                        }
                        if (toReturn[2] > 0)
                        {
                            toReturn[2] -= Cube.cubeSize + .357f;
                        }
                    }
                }
            }
            return toReturn;
        }
        public void Draw()
        {
            foreach (Block block in BlockList)
            {
                if (block != null)
                {
                    block.Draw(this.world);
                }
            }
        }
        public void addFloor()
        {
            this.BlockList = new Block[10, 10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    this.addBlock(i, 0, k, 1);
                    this.addBlock(i, 1, k, 2);
                    this.addBlock(i, 2, k, 3);
                    this.addBlock(i, 3, k, 0);
                }
            }
        }


    }
}
