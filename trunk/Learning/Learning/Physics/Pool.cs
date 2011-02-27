using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Physics
{
    class Pool<T> : Stack<T> where T : new()
    {
        public Pool() { }

        public Pool(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                Push(new T());
            }
        }

        public T Get()
        {
            if (Count > 0) return Pop();
            return new T();
        }

        public void Replace(T item)
        {
            Push(item);
        }
    }
}
