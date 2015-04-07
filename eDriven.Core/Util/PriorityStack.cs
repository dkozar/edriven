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
using System.Text;

namespace eDriven.Core.Util
{
    /// <summary>
    /// This class is used for inserting and removing TYPED items from the priority stack
    /// This kind of stack is used for purposes like cursor management
    /// In cursor management, the cursor with the greatest priority has to be shown
    /// Also, there is the need to remove any of the cursors from the queue, whenever it is needed
    /// When the cursor is being added to queue, the ID is being returned
    /// This ID is then being used for object retreival
    /// </summary>
    public sealed class PriorityStack
    {
        private readonly List<PriorityStackItem> _items = new List<PriorityStackItem>();

        private readonly ObjectPool<PriorityStackItem> _pool = new ObjectPool<PriorityStackItem>(1000);

        private int _id;

        /// <summary>
        /// Inserts the object into the priority queue
        /// Returns the ID of the same object in the queue
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        public int Insert(object obj, int priority)
        {
            //Debug.Log(string.Format("Inserting to priority stack [object: {0}; priority: {1}]", obj, priority));
            var item = _pool.Get();

            _id++;

            // NOTE: Pool used - reset all properties!
            item.Id = _id;
            item.Obj = obj;
            item.Priority = priority;

            // insert the item just after the last one with the same priority
            var lastIndex = _items.FindLastIndex(delegate(PriorityStackItem pi)
                                {
                                    return pi.Priority >= priority;
                                });

            _items.Insert(lastIndex+1, item);

            return _id;
        }

        /// <summary>
        /// Removes an item specified by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Remove(int id)
        {
            //Debug.Log("Remove: " + id);

            // check if the item already exists
            var item = _items.Find(delegate(PriorityStackItem pi)
            {
                return pi.Id == id;
            });

            if (null != item)
            {
                _items.RemoveAll(delegate (PriorityStackItem pi)
                                     {
                                         return pi.Id == id;
                                     });
                _pool.Put(item);
            }

            return item;
        }

        /// <summary>
        /// Gets the top-most item currently in the stack
        /// </summary>
        public object Current
        {
            get
            {
                if (0 == _items.Count)
                    return null;

                return _items[_items.Count-1].Obj; // return the top item
            }
        }

        /// <summary>
        /// Returns true if the stack is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _items.Count == 0;
        }

        /// <summary>
        /// Clears the stack
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        private StringBuilder _sb;

        public override string ToString()
        {
            _sb = new StringBuilder();

            var count = _items.Count;
            for (int i = count-1; i >= 0; i --)
            {
                _sb.AppendLine(string.Format("    {0}", _items[i]));
            }

            var msg = string.Format("PriorityStack [{0} items{1}]", _items.Count, null != Current ? string.Format(", current: {0}", Current) : string.Empty);
            if (_sb.Length > 0)
            {
                msg += "\n";
                msg += _sb.ToString();
            }

            return msg;
        }
    }

    internal class PriorityStackItem
    {
        internal int Id;
        internal int Priority;
        internal object Obj;

        public PriorityStackItem()
        {
        }

        public PriorityStackItem(int id, int priority, object obj)
        {
            Id = id;
            Priority = priority;
            Obj = obj;
        }

        public override string ToString()
        {
            return string.Format("Priority: {0}, Obj: {1}, [Id: {2}]", Priority, Obj, Id);
        }
    }
}