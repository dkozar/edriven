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

using eDriven.Core.Util;
using UnityEngine;

namespace eDriven.Core.Caching
{
    /// <summary>
    /// Loads images from Resources folder and caches them
    /// </summary>
    public class ImageLoader : ISyncLoader<Texture>
    {
#if DEBUG
// ReSharper disable UnassignedField.Global
        public static bool DebugMode;
// ReSharper restore UnassignedField.Global
#endif

        readonly Cache<string, Texture> _cache = new Cache<string, Texture>();

        #region Singleton

        private static ImageLoader _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private ImageLoader()
        {
            // Constructor is protected
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static ImageLoader Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating ResourceLoader instance"));
#endif
                    _instance = new ImageLoader();
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

        #region Implementation of ISyncLoader<Texture>

        private Texture _texture;

        /// <summary>
        /// Loads the image identified by path from Resources folder
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Texture Load(string id)
        {
            _texture = _cache.Get(id) ?? (Texture)Resources.Load(id, typeof(Texture));
            return _texture;
        }

        #endregion
    }
}