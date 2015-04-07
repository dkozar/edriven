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

namespace eDriven.Core.Callback
{
    /// <summary>
    /// A class that handles async calls and callbacks
    /// </summary>
    public class CallbackQueue<T>
    {
        #region Delegate definition

        /// <summary>
        /// Callback signature
        /// </summary>
        public delegate void Callback(T request);

        /// <summary>
        /// The signature of function for checking status
        /// </summary>
        public delegate bool StatusChecker(T request);

        #endregion

        #region Members

        private readonly Dictionary<T, Callback> _active;
        /// <summary>
        /// Gets currently active requests
        /// </summary>
        public Dictionary<T, Callback> Active
        {
            get
            {
                return _active;
            }
        }

        private readonly Dictionary<T, Callback> _finished;

        private StatusChecker _finishedChecker;
        /// <summary>
        /// Function for checking status
        /// </summary>
        public StatusChecker FinishedChecker
        {
            get
            {
                if (null == _finishedChecker)
                    throw new Exception("FinishedChecker function not defined");

                return _finishedChecker;
            } 
            set
            {
                _finishedChecker = value;
            }
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CallbackQueue()
        {
            _active = new Dictionary<T, Callback>();
            _finished = new Dictionary<T, Callback>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// A heartbeat function
        /// </summary>
        private void Tick()
        {
            /**
             * Look up for finished requests
             * */
            foreach (T request in _active.Keys)
            {
                if (FinishedChecker(request))
                {
                    _finished.Add(request, _active[request]);
                }
            }

            /**
             * Remove finished requests from active dictionary
             * */
            foreach (T request in _finished.Keys)
            {
                _active.Remove(request);
            }

            /**
             * Fire callbacks for finished requests
             * */
            foreach (T request in _finished.Keys)
            {
                _finished[request](request);
            }

            /**
             * Clear finished dictionary
             * */
            _finished.Clear();

            /**
             * Disconnect if no more requests
             * */
            if (_active.Count == 0)
                SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
        }

        /// <summary>
        /// Sends the request to the queue
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback">Callback to fire after the request has finished</param>
        /// <returns></returns>
        public T Send(T request, Callback callback)
        {
            if (null == callback)
                throw new Exception("Callback function not defined");

            //Debug.Log("Send: " + request + ", " + callback);
            _active.Add(request, callback);

            // connect
            SystemManager.Instance.UpdateSignal.Connect(UpdateSlot);

            return request;
        }

        /// <summary>
        /// Stops and clears current requests
        /// </summary>
        public void Reset()
        {
            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
            _active.Clear();
            _finished.Clear();
        }

        #endregion

        #region Implementation of ISlot

        private void UpdateSlot(params object[] parameters)
        {
            Tick();
        }

        #endregion
    }
}