#region License

/*
 
Copyright (c) 2012 Danko Kozar

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

using System;
using System.Collections.Generic;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The class that caches objects as key/value pairs
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Cache<TKey, TValue> where TValue : new()
    {
        /// <summary>
        /// A dictionary that holds the cached values
        /// </summary>
        private readonly Dictionary<TKey, TValue> _dict;

        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets the cached value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            if (_dict.ContainsKey(key))
                return _dict[key];

            //var value = new TValue();
            //_dict[key] = value;
            //return value;

            return default(TValue);
        }

        /// <summary>
        /// Caches the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKey key, TValue value)
        {
            if (!_dict.ContainsKey(key))
                _dict[key] = value;
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            foreach (TValue value in _dict.Values)
            {
                IDisposable disposable = (IDisposable) value;
                if (null != disposable)
                    disposable.Dispose();
            }
            _dict.Clear();
        }
    }
}