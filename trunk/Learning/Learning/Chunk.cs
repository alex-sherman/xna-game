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
        private Block[][][] BlockList;
        private World world;
        public BoundingBox hitBox;
        public Chunk(Vector3 position,World world)
        {
            this.world = world;
            this.position = position;
            this.hitBox.Min = position;
            this.hitBox.Max = position + new Vector3(10, 10, 10);
            this.addFloor();
        }
        
        public void addBlock(int x,int y,int z,int type)
        {
            Block poo = new Block(new Vector3(x + this.position.X, y + this.position.Y, z +this.position.Z),type);
            this.BlockList[x][y][z] = poo;
        }
        public void addBlock(Vector3 position, int type)
        {
            Block poo = new Block(position + this.position, type);
            this.BlockList[(int)position.X][(int)position.Y][(int)position.Z] = poo;
        }
        public Block getBlock(Ray lookat)
        {
            Block closestBlock = null;
            float? closestDistance = 100;
            float? distance;
            foreach (Block[][] row in this.BlockList)
            {
                foreach (Block[] col in row)
                {
                    foreach (Block block in col)
                    {
                        if (block != null)
                        {
                            distance = lookat.Intersects(block.hitBox);
                            if(distance!=null && distance<closestDistance && distance<=5){
                                closestBlock = block;
                                closestDistance = distance;
                            }
                        }
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
                if (this.BlockList[i][j][k] == toDestroy)
                {
                    this.BlockList[i][j][k] = null;
                    return true;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return false;
        }
        public bool destroyBlock(int i,int j, int k)
        {
            if (this.BlockList[i][j][k] !=null)
            {
                this.BlockList[i][j][k] = null;
                return true;
            }


            return false;
        }

        public Boolean[] collisionCheck(Player player)
        {
            Vector3 otherVec = player.position;
            Boolean[] toReturn = { true,true,true };
            foreach (Block[][] row in this.BlockList)
            {
                foreach (Block[] col in row)
                {
                    foreach (Block block in col)
                    {
                        if (block != null)
                        {
                            if (block.canMove(player.vLeft))
                            {
                                toReturn[0] = false;
                            }
                            if (block.canMove(player.fallBox))
                            {
                                toReturn[1] = false;
                            }
                            if (block.canMove(player.vForward))
                            {
                                toReturn[2] = false;
                            }
                        }
                    }
                }
            }
            return toReturn;
        }
        public void Draw()
        {
            foreach (Block[][] row in this.BlockList)
            {
                foreach (Block[] col in row)
                {
                    foreach (Block block in col)
                    {
                        if (block != null)
                        {
                            Cube.Draw(block.position, this.world,block.getTexture());
                        }
                    }
                }
            }
        }
        public void addFloor()
        {
            this.BlockList = new Block[10][][];
            for (int i = 0; i < 10; i++)
            {
                this.BlockList[i] = new Block[10][];
                for (int j = 0; j < 10; j++)
                {
                    this.BlockList[i][j] = new Block[10];
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    this.addBlock(i, 0, k,(i+k)%2);
                }
            }
        }
        
        
    }
}
