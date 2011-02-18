using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Learning
{
    class World
    {
        public Matrix partialWorld;
        public ArrayList chunkList = new ArrayList();
        public ArrayList players = new ArrayList();
        public World()
        {
        }
        public void addChunk(int x, int y, int z)
        {
            this.chunkList.Add(new Chunk(new Vector3(x, y, z),this));
        }
        public void addPlayer(Player player)
        {
            this.players.Add(player);
            player.world = this;
        }

        public void Update(Matrix partialWorld)
        {
            this.partialWorld = partialWorld;

        }

        public Chunk getChunk(Vector3 position)
        {
            int x = (int)(position.X/20);
            int y = (int)(position.Y/20);
            int z = (int)(position.Y/20);
            foreach (Chunk chunk in this.chunkList)
            {
                if (chunk.position.X == x && chunk.position.Y == y && chunk.position.Z == z)
                {
                    return chunk;
                }
            }
            return null;
        }
        public void Draw()
        {
            foreach(Chunk chunk in this.chunkList){
                chunk.Draw();
            }
        }
        private Boolean[] and(Boolean[] first, Boolean[] second)
        {
            
            if (first.Length != second.Length)
            {
                return first;
            }
            Boolean[] toReturn = new Boolean[first.Length];
            first.CopyTo(toReturn, 0);
            for (int i = 0; i < first.Length; i++)
            {
                toReturn[i] = first[i] && second[i];
            }
            return toReturn;
        }
        public Boolean[] collisionCheck(Player player)
        {
            Boolean[] toReturn = { true, true, true};
            foreach (Chunk chunk in this.chunkList)
            {
                toReturn = and(toReturn, chunk.collisionCheck(player));
            }
            return toReturn;
        }
    }
}
