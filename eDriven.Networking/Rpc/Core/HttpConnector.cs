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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using eDriven.Core.Events;
using eDriven.Core.Managers;
using eDriven.Core.Serialization;
using UnityEngine;
using Event=eDriven.Core.Events.Event;
using Object=UnityEngine.Object;

namespace eDriven.Networking.Rpc
{
    /// <summary>
    /// A class that handles HTTP requests/responses
    /// It can queue requests and send them one-by-one or in any other fashion specified by ConcurencyMode
    /// After the responses are returned, it processes them (fires responders) in fashion specified by ProcessingMode
    /// For instence, you could make the connector to fire all the responders, but only after all responses are returned
    /// </summary>
    public class HttpConnector : EventDispatcher, IUnique, ICloneable
    {
        #region Static

#if DEBUG
        public new static bool DebugMode;
#endif
        
        /// <summary>
        /// This event fires when number of finished requests equals total number of requests
        /// </summary>
        public static string ALL_PROCESSED = "allProcessed";
        
        /// <summary>
        /// This event fires when any request is in timeout
        /// </summary>
        public const string TIMEOUT = "timeout";

        /// <summary>
        /// Displays the overal progress
        /// </summary>
        //public static readonly HttpServiceProgressAggregator ProgressAggregator = new HttpServiceProgressAggregator();

        #endregion

        #region Properties

        #region Implementation of IUnique

        ///<summary>
        /// The identifier of this HttpConnector
        ///</summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

        #endregion

        /// <summary>
        /// The URL of the server-side
        /// If defined, one can use Send() method without parameters
        /// </summary>
        [XmlAttribute("Url")]
        public string Url { get; set; }

        /// <summary>
        /// Randomizes the URL
        /// If set to true, the URL called by this connector gets the "&=something" suffix
        /// It assures that each requests gets the non-cached version of data
        /// </summary>
        [XmlAttribute("CacheBuster")]
        public bool CacheBuster;

        /// <summary>
        /// Should calls be written to log
        /// </summary>
        [DefaultValue(false)]
        public bool LogCalls;

        /// <summary>
        /// Overal fault handler for the service
        /// </summary>
        [XmlIgnore]
        public FaultHandler FaultHandler;

        ///// <summary>
        ///// Should the response time be measured and set as Duration parameter of token?
        ///// </summary>
        //[XmlAttribute]
        //[DefaultValue(true)]
        //public bool DoMeasureTime = true;

        /// <summary>
        /// How to handle multiple calls to the same service. 
        /// The default concurrency value is multiple. 
        /// </summary>
        [XmlAttribute]
        [DefaultValue(ConcurencyMode.Multiple)]
        public ConcurencyMode ConcurencyMode = ConcurencyMode.Multiple;
        // NOTE: Null exception error happen IN EDITOR when using Multiple concurency and more than one simultaneous call!!!
        // Use Queued mode and MaxConcurrentRequests=1 when in editor! 

        /// <summary>
        /// How to handle multiple responses
        /// </summary>
        [XmlAttribute]
        [DefaultValue(ProcessingMode.Async)]
        public ProcessingMode ProcessingMode = ProcessingMode.Async;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        [XmlAttribute]
        [DefaultValue(30f)]
        public float Timeout = 30f;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        [XmlAttribute]
        [DefaultValue(false)]
        public bool ResetTimeoutOnProgress = true;

        /// <summary>
        /// Should system automatically dispose WWW resources (but after processing)
        /// </summary>
        [XmlAttribute]
        [DefaultValue(true)]
        protected bool AutoDisposeResources;

        /// <summary>
        /// You can peek to responses through this collection
        /// Available on ALL_PROCESSED event
        /// </summary>
        [XmlIgnore]
        public List<AsyncToken> Responses
        {
            get { return _forRemoval; }
        }

        ///<summary>
        /// A flag indicating that this connector is processing
        ///</summary>
        [XmlIgnore]
        public bool IsWorking { get; private set; }

        /// <summary>
        /// Max number of concurrent requests
        /// In FIFO and FILO mode
        /// Meaning it doesn't have to be processed one-by-one (the default)
        /// but two-by-two, three-by-three etc.
        /// </summary>
        [XmlAttribute]
        [DefaultValue(1)]
        public int MaxConcurrentRequests = 1;

        /// <summary>
        /// Response mode 
        /// Token or WWW (the default is token)
        /// 
        /// If ResponseMode.Token, the argument passed to result handler is AsyncToken,
        /// else if ResponseMode.WWW, the argument passed to result handler is current WWW
        /// </summary>
        public ResponseMode ResponseMode = ResponseMode.Token;

        #endregion

        #region Members

        /// <summary>
        /// Queued WWW requests
        /// When Send() is called, request is added to queue
        /// </summary>
        private readonly List<AsyncToken> _queued;

        /// <summary>
        /// Active list
        /// </summary>
        private readonly List<AsyncToken> _active;

        /// <summary>
        /// Finished list
        /// </summary>
        private readonly List<AsyncToken> _finished;

        /// <summary>
        /// List for synced modes
        /// </summary>
        private readonly List<AsyncToken> _synced;

        /// <summary>
        /// Requests ready for disposal
        /// </summary>
        private readonly List<AsyncToken> _forRemoval;

        /// <summary>
        /// Dictionary containing request to token relations
        /// </summary>
        //private readonly Dictionary<WWW, AsyncToken> _tokens;

        private int _previouslyActive;

        #endregion

        #region Default fault handler

        private static void DefaultFaultHandler(object data)
        {
            Debug.Log("Communication error: " + data);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpConnector()
        {
            _queued = new List<AsyncToken>();
            _active = new List<AsyncToken>();
            _finished = new List<AsyncToken>();

            _synced = new List<AsyncToken>();
            _forRemoval = new List<AsyncToken>();

            //ProgressAggregator.RegisterService(this);
        }

        #endregion

        #region Send method (main)

        /// <summary>
        /// Sends a request
        /// This method is always called when calling any of Send overrides
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AsyncToken Send(WebRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
                throw new Exception("URL not set");

            if (CacheBuster)
                request.CacheBuster = true;

            #region Handling relative ULRs

            string url = request.Url;

            if (url.StartsWith("~"))
            {
                url = Url + request.Url.TrimStart('~');
            }

            request.Url = url;

            #endregion

#if DEBUG
            if (DebugMode || LogCalls)
                Debug.Log(string.Format(@"* {0}
{1}", this, request));
#endif

            // create token
            AsyncToken token = new AsyncToken {Request = request, Timeout = Timeout};

            // add it to queue
            _queued.Add(token);

            if (!SystemManager.Instance.UpdateSignal.HasSlot(UpdateSlot))
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log("Connecting to UpdateSignal");
                }
#endif
                SystemManager.Instance.UpdateSignal.Connect(UpdateSlot);
            }
            
            IsWorking = true;

            DispatchStatusEvent();

            // return it to caller
            return token;
        }

        #endregion

        #region Send  overrides

        /// <summary>
        /// Sends a request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responders"></param>
        /// <returns></returns>
        public AsyncToken Send(WebRequest request, params Responder[] responders)
        {
            AsyncToken token = Send(request);
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="request"></param>
        ///<param name="resultHandlers"></param>
        ///<returns></returns>
        public AsyncToken Send(WebRequest request, params ResultHandler[] resultHandlers)
        {
            List<Responder> responders = new List<Responder>();

            foreach (ResultHandler resultHandler in resultHandlers)
            {
                responders.Add(new Responder(resultHandler));
            }

            return Send(request, responders.ToArray());
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<returns></returns>
        ///<exception cref="Exception"></exception>
        public AsyncToken Send()
        {
            if (string.IsNullOrEmpty(Url))
                throw new Exception("Url not defined");

            return Send(new WebRequest(Url));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="responders"></param>
        ///<returns></returns>
        ///<exception cref="Exception"></exception>
        public AsyncToken Send(params Responder[] responders)
        {
            if (string.IsNullOrEmpty(Url))
                throw new Exception("Url not defined");

            AsyncToken token = Send(new WebRequest(Url));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<returns></returns>
        public AsyncToken Send(string url)
        {
            return Send(new WebRequest(url));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(url));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="resultHandlers"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, params ResultHandler[] resultHandlers)
        {
            List<Responder> responders = new List<Responder>();

            foreach (ResultHandler resultHandler in resultHandlers)
            {
                responders.Add(new Responder(resultHandler));
            }

            return Send(url, responders.ToArray());
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="postData"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, byte[] postData)
        {
            return Send(new WebRequest(url, postData));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="postData"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, byte[] postData, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(url, postData));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="postData"></param>
        ///<param name="headers"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, byte[] postData, Hashtable headers)
        {
            return Send(new WebRequest(url, postData, headers));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="postData"></param>
        ///<param name="headers"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, byte[] postData, Hashtable headers, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(url, postData, headers));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="form"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, WWWForm form)
        {
            return Send(new WebRequest(url, form));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="url"></param>
        ///<param name="form"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(string url, WWWForm form, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(url, form));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="postData"></param>
        ///<returns></returns>
        public AsyncToken Send(byte[] postData)
        {
            return Send(new WebRequest(Url, postData));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="postData"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(byte[] postData, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(Url, postData));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="postData"></param>
        ///<param name="headers"></param>
        ///<returns></returns>
        public AsyncToken Send(byte[] postData, Hashtable headers)
        {
            return Send(new WebRequest(Url, postData, headers));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="postData"></param>
        ///<param name="headers"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(byte[] postData, Hashtable headers, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(Url, postData, headers));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="form"></param>
        ///<returns></returns>
        public AsyncToken Send(WWWForm form)
        {
            return Send(new WebRequest(Url, form));
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="form"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(WWWForm form, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(Url, form));
            HandleResponders(responders, token);
            return token;
        }

        ///<summary>
        /// Sends a request
        ///</summary>
        ///<param name="form"></param>
        ///<param name="headers"></param>
        ///<param name="responders"></param>
        ///<returns></returns>
        public AsyncToken Send(WWWForm form, Hashtable headers, params Responder[] responders)
        {
            AsyncToken token = Send(new WebRequest(Url, form, headers));
            HandleResponders(responders, token);
            return token;
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Cancels the request
        /// </summary>
        /// <param name="token"></param>
        public void Cancel(AsyncToken token)
        {
            // todo: execute responders! (we need this to remove progress bars etc.)
            token.Dispose();
            _queued.Remove(token);
            _active.Remove(token);
            _finished.Remove(token);
            _synced.Remove(token);
            _forRemoval.Remove(token);
            DispatchStatusEvent();
        }

        /// <summary>
        /// Cansels all requests
        /// </summary>
        public void CancelAll()
        {
            Dispose();
            DispatchStatusEvent();
        }

        #endregion

        #region Update

        /// <summary>
        /// Update slot
        /// </summary>
        /// <param name="parameters"></param>
        private void UpdateSlot(params object[] parameters)
        {
            ProcessQueued();
            ProcessActive();
            ProcessFinished();

            if (_queued.Count == 0 && _active.Count == 0 && _finished.Count == 0)
                SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
        }

        /// <summary>
        /// Processes queued requests
        /// </summary>
        private void ProcessQueued()
        {
            // transfer disposed tokens
            _queued.ForEach(delegate(AsyncToken token)
            {
                if (token.HasBeenDisposed)
                {
                    ProcessResponders(token);
                    //_finished.Add(token);
                }
            });
            _queued.RemoveAll(delegate(AsyncToken token) { return token.HasBeenDisposed; });

            if (_queued.Count > 0)
            {
                int diff = MaxConcurrentRequests - _active.Count;
                switch (ConcurencyMode)
                {
                    case ConcurencyMode.SingleLast:
                        // get the last one
                        _active.Clear(); // clear active and do not process any current responses
                        QueuedToActive(_queued[_queued.Count - 1]);
                        _queued.Clear();
                        break;

                    case ConcurencyMode.SingleFirst:
                        if (_active.Count > 0) // there are active requests already
                            _queued.Clear();
                        else if (_queued.Count > 1) // no active, but more than one queued
                        {
                            QueuedToActive(_queued[0]);
                            _queued.Clear();
                        }
                        break;

                    case ConcurencyMode.FifoQueued:
                        //if (_active.Count == 0) // if no active requests, add the first queued
                        //{
                        //    QueuedToActive(_queued[0]);
                        //}
                        if (diff > 0) // if no active requests, add the first queued
                        {
                            QueuedToActive(_queued.GetRange(0, Math.Min(diff, _queued.Count)));
                        }
                        break;

                    case ConcurencyMode.FiloQueued:
                        //if (_active.Count == 0) // if no active requests, add the last queued
                        //{
                        //    QueuedToActive(_queued[_queued.Count - 1]);
                        //}
                        if (diff > 0) // if no active requests, add the first queued
                        {
                            QueuedToActive(_queued[_queued.Count - Math.Min(diff, _queued.Count)]);
                        }
                        break;

                    case ConcurencyMode.Multiple:
                        //default:
                        // transfer ALL queued to active
                        QueuedToActive(_queued);
                        break;
                }
            }
        }

        private void QueuedToActive(AsyncToken token)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(GetInfo());
#endif


            // create WWW
            GenerateWww(token);

            _active.Add(token);

            // in Sync and SyncAll modes add it to synced list also
            if (ProcessingMode.Async != ProcessingMode)
                _synced.Add(token);

            // remove from queued
            _queued.Remove(token);

#if DEBUG
            if (DebugMode || LogCalls)
            {
                Debug.Log(string.Format(@"{0} queued --1--> {1} active", _queued.Count, _active.Count));
            }
#endif
            DispatchStatusEvent();
        }

        private void QueuedToActive(List<AsyncToken> tokens)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(GetInfo());
#endif


#pragma warning disable 168
            int count = tokens.Count;
#pragma warning restore 168

            // temp
            ICollection<AsyncToken> tempTokens = new List<AsyncToken>();

            tokens.ForEach(delegate (AsyncToken request)
                               {
                                   GenerateWww(request);
                                   tempTokens.Add(request);
                               });

            _active.AddRange(tempTokens);

            // in Sync and SyncAll modes add it to synced list also
            if (ProcessingMode.Async != ProcessingMode)
                _synced.AddRange(tempTokens);

            // clear temp
            tempTokens.Clear();

            _queued.RemoveAll(delegate(AsyncToken token) { return tokens.Contains(token); });

#if DEBUG
            if (DebugMode || LogCalls)
            {
                Debug.Log(string.Format(@"{0} queued --{1}-->{2} active", _queued.Count, count, _active.Count));
            }
#endif

            DispatchStatusEvent();
        }

        private void ProcessActive()
        {
            _previouslyActive = _active.Count;

            if (_previouslyActive > 0)
            {
#if DEBUG
                if (DebugMode){
                    Debug.Log(GetInfo());
                    Debug.Log(string.Format("# Processing {0} active", _previouslyActive));
                }
#endif

#pragma warning disable 219
                int transferCount = 0;
#pragma warning restore 219

                /**
                 * Look for requests that are done
                 * */
                _active.ForEach(delegate(AsyncToken token)
                {
                    //if (token.HasBeenDisposed)
                    //{
                    //    _finished.Add(token);
                    //    transferCount++;
                    //    return;
                    //}

                    if (ResetTimeoutOnProgress && !token.HasBeenDisposed)
                        token.CheckProgress();

                    double totalMilliseconds = DateTime.Now.Subtract(token.StartTime).TotalMilliseconds;
                    bool isTimeout = totalMilliseconds > token.Timeout*1000;

                    if (isTimeout){
                        Debug.Log(string.Format("Timeout ({0} sec)", token.Timeout));
                        token.IsTimeout = true;
                    }

                    if (token.HasBeenDisposed || token.Response.isDone || isTimeout) // NOTE: token.HasBeenDisposed has to be first check, because if it is, then token.Response is null
                    {
                        //if (DebugMode){
                        //    Debug.Log("ProcessActive: token.Response.isDone: " + token.Response.isDone);
                        //}
                        token.Duration = totalMilliseconds;

                        /**
                        * If request is done (even if in error), add it to finished requests and raise event
                        * */
                        _finished.Add(token);
                        transferCount++;
                    }
                });

                // remove all finished from _active list
                _active.RemoveAll(delegate(AsyncToken token) { return _finished.Contains(token); });

                if (_active.Count < _previouslyActive) // e.g. we moved some requests to finished
                {
#if DEBUG
                    if (DebugMode || LogCalls)
                    {
                        Debug.Log(string.Format(@"{0} active --{1}--> {2} finished", _active.Count, transferCount, _finished.Count));
                        //Debug.Log(string.Format(@"active --{0}--> finished", transferCount));
                    }
#endif
                    DispatchStatusEvent();
                }
            }
        }

        private void DispatchStatusEvent()
        {
            if (HasEventListener(HttpServiceProgressEvent.PROGRESS_CHANGE))
            {
                HttpServiceProgressEvent e = new HttpServiceProgressEvent(HttpServiceProgressEvent.PROGRESS_CHANGE)
                {
                    Active = _active.Count,
                    Finished = _finished.Count,
                    Total = _queued.Count + _active.Count + _finished.Count
                };
                DispatchEvent(e);
            }
        }

        private void ProcessFinished()
        {
            _finished.RemoveAll(delegate(AsyncToken token) { return token.HasBeenDisposed; });

            if (_finished.Count > 0)
            {
                /**
                 * Check if there are no more active requests, 
                 * and the last active request wes removed from _active collection on this update
                 * */
                bool noMoreActiveRequests = _previouslyActive > 0 && _queued.Count == 0 && _active.Count == 0; // are all requests done (no more active requests)

                // reset _previouslyActive
                _previouslyActive = 0;
                
                switch (ProcessingMode)
                {
                        /**
                     * ProcessingMode.Async
                     * Proccess requests as they are finished
                     * */
                    case ProcessingMode.Async:

                        // process all finished immediatelly
                        DoProcessAsync(noMoreActiveRequests);

                        break;

                        /**
                     * ProcessingMode.Sync
                     * We process results only if previous results in the list are processed
                     * Synced list holds active + finished members
                     * If the index in synced list and the index in finished list are the same, we should proccess the response
                     * */
                    case ProcessingMode.Sync:

                        DoProcessSync(noMoreActiveRequests);

                        break;

                        /**
                     * ProcessingMode.SyncAll
                     * Proccess responses only if there are no active requests anymore
                     * */
                    case ProcessingMode.SyncAll:

                        if (noMoreActiveRequests)
                            DoProcessSync(true);

                        break;
                }
            }
        }

        private void DoProcessSync(bool doDispatchAllProcessedEvent)
        {
#if DEBUG
            if (DebugMode)
            {
                //Debug.Log(GetInfo());
                Debug.Log(string.Format("# Processing {0} finished [synced mode])", _finished.Count));
            }
#endif


            //HandleTimeouts();

            //DispatchStatusEvent();

            //_synced.RemoveAll(delegate(AsyncToken token) // remove timeouts
            //                      {
            //                          return !token.Response.isDone;
            //                      });

            _forRemoval.Clear();

            foreach (AsyncToken token in _synced)
            {
                // checks if indexes are the same
                if (_finished.Contains(token))
                {
                    _forRemoval.Add(token);
                }
                else
                {
                    goto next; // break out of the loop as soon as non-finished www found
                }
            }

            //Debug.Log("_forRemoval: " + _forRemoval.Count);

            next: // label

            // remove all that are not synced anymore
            _synced.RemoveAll(delegate(AsyncToken token) { return _forRemoval.Contains(token); });
            _finished.RemoveAll(delegate(AsyncToken token) { return _forRemoval.Contains(token); });

            _forRemoval.ForEach(delegate(AsyncToken token)
                                    {
                                        // process WWW
                                        try
                                        {
                                            //if (DebugMode)
                                            //    Debug.Log("DoProcessSync: token.Response.isDone: " + token.Response.isDone);
                                            ProcessResponders(token);
                                        }
                                        //catch (JsonServiceException ex)
                                        //{
                                        //    // do not re-throw
                                        //    // this request should be removed
                                        //    Debug.Log(ex);
                                        //}
                                        catch (ArgumentNullException ex)
                                        {
                                            Debug.Log(ex);
                                        }
                                    });

            if (doDispatchAllProcessedEvent)
            {
                _synced.Clear();
                _finished.Clear();

#if DEBUG
            if (DebugMode )
                {
                    Debug.Log("All finished");
                    //Debug.Log(string.Format(@"active --{0}--> finished", transferCount));
                }
#endif

                if (LogCalls)
                {
                    Debug.Log("All finished");
                    //Debug.Log(string.Format(@"active --{0}--> finished", transferCount));
                }

                IsWorking = false;
                // dispatch ALL_PROCESSED event
                DispatchEvent(new Event(ALL_PROCESSED));
            }

            if (AutoDisposeResources)
            {
                // disposal
                _forRemoval.ForEach(delegate(AsyncToken d)
                                        {
                                            DisposeResources(d.Response);
                                        });
            }

            _forRemoval.Clear();

            DispatchStatusEvent();

#if DEBUG
            if (DebugMode)
                Debug.Log(GetInfo());
#endif

        }

        private void DoProcessAsync(bool doDispatchAllProcessedEvent)
        {
#if DEBUG
            if (DebugMode)
            {
                //Debug.Log(GetInfo());
                Debug.Log(string.Format("# Processing {0} finished", _finished.Count));
            }
#endif
            //HandleTimeouts();

            // prepare for list removal
            _forRemoval.Clear();

            _forRemoval.AddRange(_finished);

            //DispatchStatusEvent();

            // clear finished
            _finished.Clear();

            // process responders
            _forRemoval.ForEach(delegate(AsyncToken token)
                                    {
                                        //if (DebugMode)
                                        //    Debug.Log("DoProcessAsync: token.Response.isDone: " + token.Response.isDone);
                                        ProcessResponders(token);
                                    });

            // dispatch ALL_PROCESSED event
            if (doDispatchAllProcessedEvent)
            {
                IsWorking = false;
                DispatchEvent(new Event(ALL_PROCESSED));
            }

            // disposal
            if (AutoDisposeResources)
                _forRemoval.ForEach(delegate(AsyncToken token) { DisposeResources(token.Response); });

            // now we can clear _forRemoval collection
            _forRemoval.Clear();
            
            DispatchStatusEvent();

#if DEBUG
            if (DebugMode)
                Debug.Log(GetInfo());
#endif

        }

        #region _crap

//private void HandleTimeouts()
        //{
        //    List<AsyncToken> timeoutRequests = _finished.FindAll(delegate(AsyncToken token) { return !token.Response.isDone; });

        //    timeoutRequests.ForEach(delegate(AsyncToken token)
        //                                {
        //                                    token.Responders.ForEach(delegate(Responder responder)
        //                                                                 {
        //                                                                     responder.Fault("Request timeout");
        //                                                                 });
        //                                });

        //    if (timeoutRequests.Count > 0)
        //        DispatchEvent(new Event(TIMEOUT));

        //    // remove responses that have time out
        //    _finished.RemoveAll(delegate(AsyncToken token)
        //                            {
        //                                return !token.Response.isDone;
        //                            });
        //}

        #endregion

        private void ProcessResponders(AsyncToken token)
        {
            if (token.IsTimeout){
                DispatchEvent(new Event(TIMEOUT));
                token.Response.Dispose();
            }

            /* Timeout */
            if (token.IsTimeout /*|| !token.Response.isDone*/) // response never returned, timeout expired
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log("1 request timeout");
                }
#endif
                token.Responders.ForEach(delegate(Responder responder)
                                             {
                                                 // If no fault handler defined for the responder, 
                                                 // and one is defined globally
                                                 // reference that handler
                                                 if (null == responder.FaultHandler)
                                                 {
                                                     responder.FaultHandler = FaultHandler ?? DefaultFaultHandler;
                                                 }

                                                 responder.Fault("Request timeout");
                                             });
            }

            /* Error */
            else if (!string.IsNullOrEmpty(token.Response.error)) // NOTE: Response could have isDone == true and still have and error!
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log("Error: " + token.Response.error);
                }
#endif
                token.Responders.ForEach(delegate(Responder responder)
                                             {
                                                 // If no fault handler defined for the responder, 
                                                 // and one is defined globally
                                                 // reference that handler
                                                 if (null == responder.FaultHandler)
                                                 {
                                                     responder.FaultHandler = FaultHandler ?? DefaultFaultHandler;
                                                 }

                                                 responder.Fault(token.Response.error);
                                             });
            }

            /* OK */
            else
            {
                //token.Result = token.Response; // add result to token (for alternative processing)

                //Debug.Log(1);

                token.Responders.ForEach(delegate(Responder responder)
                                             {
                                                 try {
                                                     //if (DebugMode)
                                                     //    Debug.Log("ProcessResponders: token.Response.isDone: " + token.Response.isDone);

                                                     //Debug.Log(2);

                                                     switch (ResponseMode)
                                                     {
                                                         case ResponseMode.Token:
                                                             responder.Result(token);
                                                             break;
                                                         case ResponseMode.WWW:
                                                             responder.Result(token.Response);
                                                             break;
                                                     }

                                                     //responder.Result((ResponseMode == ResponseMode.Token) ? token : token.Response);
                                                 }
                                                 catch (Exception ex)
                                                 {
                                                     Debug.Log("Error executing result responder: " + ex); // NOTE: Silent fail
                                                 }
                                             });
            }

            // clear responders
            token.ClearResponders();
        }

        /// <summary>
        /// Disposes resources on WWW
        /// </summary>
        /// <param name="response"></param>
        public static void DisposeResources(WWW response)
        {
            //if (Application.isEditor) // Edit mode
            //{
            //    Object.DestroyImmediate(response.audioClip);
            //    Object.DestroyImmediate(response.movie);
            //    Object.DestroyImmediate(response.texture);
            //}

            //else // app mode
            //{
            //    Object.Destroy(response.audioClip);
            //    Object.Destroy(response.movie);
            //    Object.Destroy(response.texture);
            //}

            response.Dispose();

            //response = null;
            //System.GC.Collect();
        }

        public override void Dispose()
        {
            base.Dispose();

            _queued.ForEach(delegate(AsyncToken token) { token.Dispose(); });
            _active.ForEach(delegate(AsyncToken token) { token.Dispose(); });
            _finished.ForEach(delegate(AsyncToken token) { token.Dispose(); });

            _queued.Clear();
            _active.Clear();
            _finished.Clear();
            _synced.Clear();
            _forRemoval.Clear();

            IsWorking = false;

            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
        }

        //public void DisposeAllResponses()
        //{
        //    _active.ForEach(delegate(AsyncToken token)
        //                        {
        //                            DisposeResources(token.Response);
        //                        });
        //}

        #endregion

        #region Helper

        private void GenerateWww(AsyncToken token)
        {
            token.Response = token.Request.CreateWww(); // the WWW request is now alive!
            token.Timeout = Timeout;
            token.StartTime = DateTime.Now;
        }

        private static void HandleResponders(IEnumerable<Responder> responders, AsyncToken token)
        {
            foreach (Responder responder in responders)
            {
                token.AddResponder(responder);
            }
        }

        private string GetInfo()
        {
            return string.Format(@"[{0} queued; {1} active; {2} finished; ({3} synced;)]{4}", _queued.Count, _active.Count, _finished.Count, _synced.Count,  
                                 string.IsNullOrEmpty(Id) ? "" : string.Format(" //{0}", Id));
        }

        #endregion

        public override string ToString()
        {
            return string.Format(@"[HttpConnector [Id=""{0}"", Url=""{1}"", ConcurencyMode=""{2}"", ProcessingMode=""{3}"", MaxConcurrentRequests=""{4}"", Timeout={5}]]
Queued: {6}; Active: {7}; Finished: {8}", Id, Url, ConcurencyMode, ProcessingMode, MaxConcurrentRequests, Timeout, _queued.Count, _active.Count, _finished.Count);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            return MemberwiseClone(); // shalow copy
        }

        ///<summary>
        /// Stops all requests from processing
        ///</summary>
        public void StopAll()
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log("HttpConnector->StopAll");
            }
#endif
            Dispose();
        }
    }
}