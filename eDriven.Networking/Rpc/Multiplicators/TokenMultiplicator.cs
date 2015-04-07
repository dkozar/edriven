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
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace eDriven.Networking.Rpc.Multiplicators
{
    /// <summary>
    /// HttpLoader extension handling the ONE-TO-MANY token mapping
    /// Used for loading multiple resources from a few URLs
    /// This class introduces a robust mechanism for doing stuff like that
    /// It will give the resource if it already has it
    /// If it hasn't start loading yet, it will start the HTTP download and return a token (ONE-TO-MANY token mapping)
    /// If you request the resource that is currently being loaded, it maps to already started HTTP download (ONE-TO-MANY token mapping)
    /// </summary>
    /// <typeparam name="T">The type to extract from the response</typeparam>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class TokenMultiplicator<T> where T : Object
    {
        #region Delegates

        public delegate T ResponseExtractorDelegate(WWW response);
        public delegate void TokenUpdaterDelegate(AsyncToken token, T output);

        #endregion

        #region Properties

        private HttpConnector _connector;
        /// <summary>
        /// HttpConnector for handling actual download
        /// </summary>
        public HttpConnector Connector
        {
            get
            {
                if (null == _connector)
                    throw new Exception("Connector not defined");
                
                return _connector;
            }
            set
            {
                _connector = value;
            }
        }

        private ResponseExtractorDelegate _responseExtractor;
        /// <summary>
        /// Extracts interesting data from the response
        /// </summary>
        public ResponseExtractorDelegate ResponseExtractor
        {
            get
            {
                if (null == _responseExtractor)
                    throw new Exception("PostLoadProcessor not defined");

                return _responseExtractor;
            }
            set
            {
                _responseExtractor = value;
            }
        }

        private TokenUpdaterDelegate _tokenUpdater;
        /// <summary>
        /// Updates a token after the response is received
        /// </summary>
        public TokenUpdaterDelegate TokenUpdater
        {
            get
            {
                if (null == _tokenUpdater)
                    throw new Exception("PostLoadTokenUpdater not defined");

                return _tokenUpdater;
            }
            set
            {
                _tokenUpdater = value;
            }
        }

        private TokenUpdaterDelegate _settings;
        /// <summary>
        /// Extraction settings
        /// </summary>
        public TokenUpdaterDelegate Settings
        {
            get
            {
                if (null == _settings)
                    throw new Exception("Settings not defined");

                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        #endregion

        #region Members

        /// <summary>
        /// Dictionary connecting token and callback
        /// </summary>
        private readonly Dictionary<AsyncToken, ResultHandler> _callbacks = new Dictionary<AsyncToken, ResultHandler>();

        /// <summary>
        /// Active downloads
        /// </summary>
        private readonly Dictionary<string, AsyncToken> _active = new Dictionary<string, AsyncToken>();

        /// <summary>
        /// Dictionary connecting http token and multiplied tokens
        /// </summary>
        private readonly Dictionary<AsyncToken, List<AsyncToken>> _tokenToTokens = new Dictionary<AsyncToken, List<AsyncToken>>();

        /// <summary>
        /// Loaded objects
        /// </summary>
        private readonly Dictionary<string, T> _finished = new Dictionary<string, T>();

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the resource is loaded
        /// </summary>
        /// <param name="path">The path to load from</param>
        /// <returns></returns>
        public bool HasLoaded(string path)
        {
            return _finished.ContainsKey(path);
        }

        /// <summary>
        /// Gets already loaded resource
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns></returns>
        public T Get(string path)
        {
            if (!_finished.ContainsKey(path))
                throw new Exception(string.Format(@"Resource on path ""{0}"" not yet loaded. 
You should ask me nicely if I have the resource loaded using the Loaded() method before you get something ;)", path));

            return _finished[path];
            //return _loadedTextures.ContainsKey(texturePath) ? _loadedTextures[texturePath] : null;
        }

        /// <summary>
        /// Initializes download
        /// </summary>
        /// <param name="path">The path to load from</param>
        /// <param name="callback">Fires when resource is loaded</param>
        /// <returns></returns>
        public AsyncToken Load(string path, ResultHandler callback)
        {
            //Debug.Log(string.Format(@"*** Loading texture ""{0}""", textureName));

            AsyncToken httpToken;

            if (_finished.ContainsKey(path)) // texture already loaded
            {
                /**
                 * Return null to signalize there's nothing for async loading
                 * */
                return null;
            }

            /**
             * The texture not yet loaded, but loading
             * We need to return token (for displaying progress)
             * */
            if (_active.ContainsKey(path))
            {
                /**
                 * The token collection for this texture already exists
                 * We should grab the first token and clone it
                 * */
                httpToken = _active[path];
            }
            else
            {
                //Debug.Log("*** Downloading texture: " + texturePath);
                httpToken = _connector.Send("~" + path, LoadCompleteHandler);
                httpToken.Data = path;

                _active.Add(path, httpToken);

                _tokenToTokens.Add(httpToken, new List<AsyncToken>());
            }

            AsyncToken token = (AsyncToken)httpToken.Clone();
            _tokenToTokens[httpToken].Add(token);

            _callbacks.Add(token, callback);

            return token;
        }

        /// <summary>
        /// Cancels the current download
        /// </summary>
        /// <param name="token"></param>
        public void Cancel(AsyncToken token)
        {
            _connector.Cancel(token);
        }

        /// <summary>
        /// Cancels all downloads
        /// </summary>
        public void CancelAll()
        {
            _connector.CancelAll();
        }

        /// <summary>
        /// Unloads a texture
        /// </summary>
        /// <param name="texturePath"></param>
        public void Unload(string texturePath)
        {
            if (_finished.ContainsKey(texturePath))
            {
                Object.Destroy(_finished[texturePath]);
                _finished.Remove(texturePath);
            }

            #region _crp

            //if (this._downloads.ContainsKey(TextureURL))
            //{
            //    Debug.Log("Disposing web request..." + TextureURL);
            //    this._downloads[TextureURL].Dispose();
            //    this._downloads[TextureURL] = null;
            //    this._downloads.Remove(TextureURL);
            //}

            #endregion

        }

        #endregion

        #region Handlers

        /// <summary>
        /// Executed after the download is finished
        /// </summary>
        /// <param name="data"></param>
        private void LoadCompleteHandler(object data)
        {
            //Debug.Log("LoadCompleteHandler");

            AsyncToken httpToken = (AsyncToken)data;

            string path = (string)httpToken.Data;

            T output = ResponseExtractor(httpToken.Response);
            
            _finished.Add(path, output);

            _tokenToTokens[httpToken].ForEach(delegate(AsyncToken token)
                                                  {
                                                      token.Response = httpToken.Response;

                                                      if (null != _tokenUpdater)
                                                        TokenUpdater(token, output);

                                                      /**
                                                        * Execute callback function
                                                        * */
                                                      if (_callbacks.ContainsKey(token))
                                                      {
                                                          ResultHandler resultHandler = _callbacks[token];
                                                          _callbacks.Remove(token);
                                                          resultHandler(token);
                                                      }
                                                  });

            _tokenToTokens.Remove(httpToken);

            _active.Remove(path);

        }

        #endregion

    }
}