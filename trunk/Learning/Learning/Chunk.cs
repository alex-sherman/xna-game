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
        public static Chunk center;
        private Vector3 position;
        private Block[][][] BlockList;

       
        public Chunk(Vector3 position)
        {
            this.position = position;
            this.addFloor();
        }
        public static void drawChunks(Matrix partialWorld, Vector3 cameraPos)
        {
            Chunk.center.Draw(partialWorld, cameraPos);
        }
        public static Chunk getChunk(Vector3 position)
        {
            return Chunk.center;
        }
        public static void addChunk(int x, int y, int z)
        {
            Chunk.center = new Chunk(new Vector3(x, y, z));
        }
        public void addBlock(int x,int y,int z)
        {
            Block poo = new Block(new Vector3(x + 10 * this.position.X, y + 10 * this.position.Y, z + 10 * this.position.Z));
            this.BlockList[x][y][z] = poo;
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
        public void Draw(Matrix partialWorld, Vector3 cameraPos)
        {
            foreach (Block[][] row in this.BlockList)
            {
                foreach (Block[] col in row)
                {
                    foreach (Block block in col)
                    {
                        if (block != null)
                        {
                            Cube.Draw(block.position, partialWorld, cameraPos);
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
                    this.addBlock(i, 0, k);
                }
            }
        }
        
        
    }
}
