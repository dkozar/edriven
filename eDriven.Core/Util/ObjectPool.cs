#region License

/*
 
Copyright (c) 2010-2014 Danko Kozar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 
*/

#endregion License

using System.Collections.Generic;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The generic object pool
    /// Used for making the objects reusable
    /// This is the efficient anti-measure for memory leaks
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : new()
    {
        private readonly Queue<T> _queue = new Queue<T>();

        private int _poolSize = 1000;
        /// <summary>
        /// The maximum number of objects that this pool can hold
        /// </summary>
        public int PoolSize
        {
            get
            {
                return _poolSize;
            }
            set
            {
                _poolSize = value; // TODO: A case when pool size is changed dinamically when holding objects
                //if (_queue.Count > _poolSize) {}
            }
        }

        #region Constructors

        /// <summary>
        /// Constructor for a pool with default size
        /// </summary>
        public ObjectPool()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="poolSize">Pool size</param>
        public ObjectPool(int poolSize)
        {
            _poolSize = poolSize;
        }

        #endregion

        /// <summary>
        /// Adds an object to the pool
        /// </summary>
        /// <param name="obj">Object to be added</param>
        public void Put(T obj)
        {
            if (_queue.Count >= PoolSize)
                return;

            _queue.Enqueue(obj);
        }

        /// <summary>
        /// Puts a list of objects into the pool
        /// </summary>
        /// <param name="objects">Objects to be added</param>
        public void Put(List<T> objects)
        {
            objects.ForEach(delegate (T o)
                                {
                                    if (_queue.Count >= PoolSize)
                                        return;

                                    _queue.Enqueue(o);
                                });
        }

        /// <summary>
        /// Releases an object from the pool
        /// </summary>
        /// <returns>The object from the pool, or a new created one if a pool is empty</returns>
        public T Get()
        {
            if (_queue.Count != 0)
            {
                return _queue.Dequeue();
            }

            return new T();
        }

        /// <summary>
        /// Returns the current count
        /// </summary>
        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }

        public override string ToString()
        {
            return string.Format(@"ObjectPool<{0}> [Holding: {1}/{2}]", typeof(T), _queue.Count, PoolSize);
        }
    }
}