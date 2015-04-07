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

using eDriven.Core.Caching;
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// Resource loader proxy
    /// Caches the once-loaded resources
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceLoader<T> : ISyncLoader<T> where T:class, new()
    {
#if DEBUG
        // ReSharper disable UnassignedField.Global
        /// <summary>
        /// Is debug mode on
        /// </summary>
        public static bool DebugMode;
        // ReSharper restore UnassignedField.Global
#endif

        #region Singleton

        private static ResourceLoader<T> _instance;

        /// <summary>
        /// Singleton class for handling focus
        /// </summary>
        private ResourceLoader()
        {
            // Constructor is protected
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static ResourceLoader<T> Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating ResourceLoader instance"));
#endif
                    _instance = new ResourceLoader<T>();
                    Initialize();
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the Singleton instance
        /// </summary>
        private static void Initialize()
        {

        }

        private readonly Cache<string, T> _cache = new Cache<string, T>();

        private T _t;

        #region Implementation of ISyncLoader<T>

        /// <summary>
        /// Loads the texture specified by ID
        /// </summary>
        /// <param name="id">ID</param>
        public T Load(string id)
        {
            _t = _cache.Get(id);
            if (null != _t)
                return _t;

            _t = Resources.Load(id) as T;
            _cache.Put(id, _t); // cache it once

            return _t;
        }

        #endregion
    }
}
