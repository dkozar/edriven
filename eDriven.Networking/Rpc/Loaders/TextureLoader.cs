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

using System;
using eDriven.Core.Caching;
using eDriven.Core.Util;
using eDriven.Networking.Util;
using UnityEngine;

namespace eDriven.Networking.Rpc.Loaders
{
    /// <summary>
    /// Loads textures from web using the internal HttpConnector
    /// Caches responses, and returns the cached value when retrieved for the second time
    /// </summary>
    public class TextureLoader : IAsyncLoader<Texture>
    {
        public HttpConnector Connector = new HttpConnector
                                                                {
                                                                    ConcurencyMode = ConcurencyMode.Multiple,
                                                                    ProcessingMode = ProcessingMode.Async,
                                                                    ResponseMode = ResponseMode.WWW
                                                                };

        readonly static Cache<string, Texture> ImageCache = new Cache<string, Texture>();

        private bool _cache;
        public bool Cache
        {
            get
            {
                return _cache;
            }
            set
            {
                if (value == _cache) 
                    return;

                if (!_cache)
                    ImageCache.Clear();

                _cache = value;
            }
        }

        public void Load(string id, AsyncLoaderCallback<Texture> callback)
        {
            Texture cashedTexture = _cache ? ImageCache.Get(id) : null;
            if (null != cashedTexture)
            {
                callback(cashedTexture);
            }
            else
            {
                Connector.Send(
                    new WebRequest(id),
                    new Responder(
                        delegate(object data)
                        {
                            WWW www = (WWW)data;

                            var texture = new Texture2D(200, 150);
                            www.LoadImageIntoTexture(texture);
                            if (_cache)
                                ImageCache.Put(id, texture);
                            callback(texture);
                        },
                        delegate(object data)
                        {
                            Logger.Log("Image loading failed: " + id);
                            throw new Exception("Loading error:\n\n" + data);
                        }
                    )
                );
            }
        }
    }
}