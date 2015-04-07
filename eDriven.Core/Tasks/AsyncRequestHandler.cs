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
using eDriven.Core.Managers;
using UnityEngine;

namespace eDriven.Core.Tasks
{
    /// <summary>
    /// Handles task callbacks
    /// </summary>
    public class AsyncRequestHandler<TIdentifier, TResponse> : IDisposable
    {
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;

        #region Delegate definition

        /// <summary>
        /// Callback signature
        /// </summary>
        public delegate void Callback(TResponse response);

        /// <summary>
        /// The signature of function to check status
        /// </summary>
        public delegate TIdentifier IdResolver(TResponse response);

        #endregion

        #region Members

        /// <summary>
        /// Dictionary [TIdentifier, Callback]
        /// </summary>
        private readonly Dictionary<TIdentifier, Callback> _dictId;

        /// <summary>
        /// Dictionary [TResponse, Callback]
        /// </summary>
        private readonly Dictionary<TResponse, Callback> _dictResponse;

        /// <summary>
        /// Finished IDs
        /// </summary>
        //private readonly List<TIdentifier> _finishedIds;

        /// <summary>
        /// Unprocessed responses
        /// </summary>
        private readonly List<TResponse> _responses;

        /// <summary>
        /// Finished responses
        /// </summary>
        //private readonly List<TResponse> _finishedResponses;

        private IdResolver _getId;
        // ReSharper disable MemberCanBePrivate.Global
        /// <summary>
        /// Delegate for getting ID from response
        /// </summary>
        public IdResolver GetId
            // ReSharper restore MemberCanBePrivate.Global
        {
            get
            {
                if (null == _getId)
                    throw new Exception("GetId function not defined");

                return _getId;
            } 
            set
            {
                _getId = value;
            }
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncRequestHandler()
        {
            _dictId = new Dictionary<TIdentifier, Callback>();
            _dictResponse = new Dictionary<TResponse, Callback>();

            //_finishedIds = new List<TIdentifier>();
            
            _responses = new List<TResponse>();
            //_finishedResponses = new List<TResponse>();

            /**
             * Subscribe to system manager
             * */
            SystemManager.Instance.UpdateSignal.Connect(Receive);
        }

        #endregion

        #region Methods

        /// <summary>
        /// A heartbeat function
        /// </summary>
        public void Process(/*Event e*/)
        {
            if (0 == _responses.Count)
                return; // early return if nothnig to process

            /**
             * Look up for unprocessed responses
             * */
            foreach (TResponse response in _responses)
            {
                TIdentifier id = GetId(response);

#if DEBUG
                if (DebugMode)
                    Debug.Log(string.Format("Processing response with ID[{0}]", id));
#endif

                if (_dictId.ContainsKey(id))
                {
                    Callback callback = _dictId[id];

                    _dictId.Remove(id);
                    _dictResponse.Add(response, callback);

#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Response [{0}] processed", id));
#endif

                }
            }

            /**
             * Fire callbacks with response as parameter
             * */
            foreach (KeyValuePair<TResponse, Callback> pair in _dictResponse)
            {
                pair.Value(pair.Key);
            }

            /**
             * Clear response dict
             * */
            _dictResponse.Clear();
            _responses.Clear();
        }

// ReSharper disable MemberCanBeProtected.Global
        /// <summary>
        /// Sends a request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public virtual void Send(TIdentifier request, Callback callback)
// ReSharper restore MemberCanBeProtected.Global
        {
            //Debug.Log("Send: " + request + ", " + callback);

            _dictId.Add(request, callback);
        }

        /// <summary>
        /// Adds a response
        /// </summary>
        /// <param name="response"></param>
        public void AddResponse(TResponse response){
            _responses.Add(response);
        }

        #endregion

        #region Implementation of ISlot

        /// <summary>
        /// Update signal reveicer
        /// </summary>
        /// <param name="parameters"></param>
        public void Receive(params object[] parameters)
        {
            Process();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            SystemManager.Instance.UpdateSignal.Disconnect(Receive);
        }

        #endregion
    }
}