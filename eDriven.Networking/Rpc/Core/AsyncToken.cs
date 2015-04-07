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
using eDriven.Core.Tasks;
using UnityEngine;

namespace eDriven.Networking.Rpc
{
    public class AsyncToken : ICloneable, IProgress, IDisposable
    {
        #region Properties

        private readonly List<Responder> _responders = new List<Responder>();
        internal List<Responder> Responders
        {
            get
            {
                return _responders;
            }
        }

        /// <summary>
        /// Request -> response duration
        /// </summary>
        public double Duration { get; internal set; }

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public float Timeout;

        /// <summary>
        /// WWW request is in timeout
        /// </summary>
        public bool IsTimeout;

        public void AddResponder(Responder responder)
        {
            _responders.Add(responder);
        }

        public void ClearResponders()
        {
            _responders.ForEach(delegate (Responder r)
                                    {
                                        r.ResultHandler = null;
                                        r.FaultHandler = null;
                                    });
            _responders.Clear();
        }

        public bool HasResponder()
        {
            return _responders.Count > 0;
        }

        public WebRequest Request { get; internal set;}

        public WWW Response { get; set; }

        public AsyncToken()
        {
            Original = this;
        }

        /// <summary>
        /// Call this method if you wish to reset time (used for timeout) 
        /// in the mode of calculating timeout for "stucked" downloads only (e.g. that have no progress)
        /// </summary>
        public void CheckProgress()
        {
            //if (null == Response) // 20120502
            //    return;

            if (Response.progress > _lastProgress)
            {
                StartTime = DateTime.Now;
                _lastProgress = Response.progress;
            }
        }

        public float Progress
        {
            get
            {
                return null != Original.Response ? Original.Response.progress : 0;
            }
        }

        private AsyncToken _original;
        /// <summary>
        /// If token has been cloned, the original token was set here by Clone() method
        /// </summary>
        internal AsyncToken Original
        {
            get
            {
                return _original;
            }
            set
            {
                _original = value;
            }
        }

        //private AsyncToken _root;
        ///// <summary>
        ///// The root token from which this one has been cloned
        ///// </summary>
        //public AsyncToken Root
        //{
        //    get
        //    {
        //        return _root;
        //    }
        //    set
        //    {
        //        _root = value;
        //    }
        //}

        /// <summary>
        /// The result of the WWW request
        /// </summary>
        //public object Result { get; internal set; }

        /// <summary>
        /// Additional token data
        /// Could be added by developer
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region Members

        private float _lastProgress;

        /// <summary>
        /// Request start time (used for calculating Duration)
        /// </summary>
        internal DateTime StartTime;
        
        #endregion

        #region Methods

        /// <summary>
        /// Clones the token (without responders)
        /// Copies Data
        /// Sets this as original token
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            AsyncToken original = Original ?? this;

            AsyncToken token = new AsyncToken
                                   {
                                       Original = original,
                                       Request = original.Request,
                                       Response = original.Response,
                                       StartTime = original.StartTime,
                                       Data = Data
                                   };
            return token;
        }

        #endregion

        public bool HasBeenDisposed { get; private set; }

        public void Dispose()
        {
            if (null != Request)
                Request.Dispose();

            Request = null;
            Response = null;
            HasBeenDisposed = true;
        }
    }
}