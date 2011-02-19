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
        public ArrayList itemList = new ArrayList();
        public World()
        {

        }
        public void spawnItem(int type, Vector3 position)
        {
            Item poo = new Item(position, type);
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
            foreach(Player player in this.players){
                //player.Update();
            }
            Item.Update(this);
            this.partialWorld = partialWorld;

        }
        public bool addBlock(Ray lookat, int type)
        {

            Chunk newChunk;
            foreach (Chunk chunk in this.chunkList)
            {
                if (lookat.Intersects(chunk.hitBox) != null)
                {
                    Block hit = chunk.getBlock(lookat);
                    if (hit != null )
                    {
                        newChunk = this.getChunk(hit.getNormal(lookat) + hit.position);
                        if (newChunk != null)
                        {
                            
                            newChunk.addBlock(hit.getNormal(lookat) + hit.position - newChunk.position, type);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void destroyBlock(Ray lookat)
        {
            foreach (Chunk chunk in this.chunkList)
            {
                if (lookat.Intersects(chunk.hitBox) != null)
                {
                    Block destroyed = chunk.getBlock(lookat);
                    if (destroyed != null)
                    {
                        this.spawnItem(destroyed.type, destroyed.position);
                        chunk.destroyBlock(destroyed);
                        return;
                    }
                }
            }

        }
        public Chunk getChunk(float i, float j, float k)
        {
            Vector3 point = new Vector3(i, j, k);
            foreach (Chunk chunk in this.chunkList)
            {
                if (chunk.hitBox.Contains(point) == ContainmentType.Contains)
                {
                    return chunk;
                }
            }
            return null;
        }
        public Chunk getChunk(Vector3 pos)
        {
            float i = pos.X;
            float j = pos.Y;
            float k = pos.Z;
            foreach (Chunk chunk in this.chunkList)
            {
                if (chunk.position.X + 10 > i && chunk.position.X <= i &&
                    chunk.position.Y + 10 > j && chunk.position.Y <= j &&
                    chunk.position.Z + 10 > k && chunk.position.Z <= k)
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
            Item.Draw(this);
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
