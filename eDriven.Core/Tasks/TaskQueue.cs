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
// ReSharper disable ClassNeverInstantiated.Global
    /// <summary>
    /// Task queue
    /// </summary>
    public class TaskQueue : IDisposable
// ReSharper restore ClassNeverInstantiated.Global
    {
        #region Static

        public static bool DebugMode = true;

        #endregion

        #region Delegate definition

        /// <summary>
        /// Callback signature
        /// </summary>
        public delegate void Callback(ITask job);

        #endregion

        #region Properties

        public bool IsWorking { get; private set; }

        public bool AutoUpdate = true;

        /// <summary>
        /// All jobs
        /// </summary>
        private readonly List<ITask> _jobs = new List<ITask>();
        public List<ITask> Jobs
        {
            get { return _jobs.FindAll(delegate(ITask job) { return !job.Excluded; }); }
        }
        
        private ITask _current;
        public ITask Current
        {
            get { return _current; }
        }

        public int Count
        {
            get { return Jobs.Count; }
        }

        private int _currentIndex = -1;
        public int CurrentIndex
        {
            get { return _currentIndex; }
        }

        public IProgress CurrentProgress
        {
            get { return null != _current ? _current.Token : null; }
        }

        public Callback SingleFinishedHandler { get; set; }

        public Callback AllFinishedHandler { get; set; }

        #endregion
        
        #region Members

        /// <summary>
        /// Queued jobs
        /// When Run() is called, a job is added to the queue
        /// </summary>
        private readonly List<ITask> _queued = new List<ITask>();
        
        #endregion

        #region Constructor

        public TaskQueue()
        {
            
        }

        public TaskQueue(Callback callback)
        {
            AllFinishedHandler = callback;
        }

        #endregion

        #region Methods

        public void Add(params ITask[] jobs)
        {
            foreach (ITask job in jobs)
            {
                // add it to queue
#if DEBUG
                if (DebugMode)
                    Debug.Log("Adding job: " + job);
#endif

                //_queued.Add(job);
                _jobs.Add(job);
            }
        }

        public void Run(params ITask[] jobs)
        {
            Add(jobs);

            // add it to queue
#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format(@"######### STARTING [{0}] JOBS #########", _queued.Count));
#endif

            _currentIndex = -1;

            _queued.AddRange(_jobs);

            if (_queued.Count > 0)
            {
                IsWorking = true;
                if (AutoUpdate)
                    //SystemManager.Instance.AddEventListener(SystemManager.UPDATE, OnUpdate);
                    SystemManager.Instance.UpdateSignal.Connect(UpdateSlot);
            }
        }
        
        public void Reset()
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("### RESETTING ###");
#endif
                    
            IsWorking = false; // set the flag

            if (AutoUpdate)
                //SystemManager.Instance.RemoveEventListener(SystemManager.UPDATE, OnUpdate); // disconnect from SystemManager
                SystemManager.Instance.UpdateSignal.Connect(UpdateSlot);

            _current = null; // nullify the reference
            _currentIndex = -1;
            _queued.Clear();
            _jobs.Clear();
        }

        public void Tick()
        {
            ProcessQueued();
            ProcessActive();
        }

        #endregion

        #region Private methods

        //private void OnUpdate(Event e)
        //{
        //    Tick();
        //}

        private void ProcessQueued()
        {
            if (_queued.Count > 0 && null == _current) // check if no current processing
            {
                _current = _queued[0]; // take the first one
                _currentIndex++;

                _queued.Remove(_current); // remove it from the queue

#if DEBUG
                if (DebugMode)
                    Debug.Log(string.Format(@"######### STARTING job [{0}] #########", _current));
#endif

                _current.Run(); // run it
            }
        }

        private void ProcessActive()
        {
            if (null != _current)
            {
                _current.Tick(); // heartbeat

                if (_current.IsDone) // if current is done
                {
                    /**
                     * If callback for a job is defined, execute it
                     * */
                    if (null != _current.Callback)
                        _current.Callback(_current);

                    /**
                     * If handler for a SINGLE ITEM FINISHED is defined, execute it
                     * */
                    if (null != SingleFinishedHandler)
                        SingleFinishedHandler(_current);

#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format(@"######### Ending job [{0}] after {1} ms #########", _current,
                                                (DateTime.Now.Subtract(_current.StartTime).TotalMilliseconds)));
#endif

                    if (_queued.Count == 0) // if no requests to process
                    {
#if DEBUG
                        if (DebugMode)
                            Debug.Log("### ALL JOBS DONE ###");
#endif
                        /**
                         * If handler for a WHOLE QUEUE is defined, execute it
                         * */
                        if (null != AllFinishedHandler)
                            AllFinishedHandler(_current);

                        IsWorking = false; // set the flag

                        if (AutoUpdate)
                            SystemManager.Instance.UpdateSignal.Connect(UpdateSlot); // disconnect from SystemManager
                    }

                    _current = null; // nullify the reference
                }
            }
        }

        #endregion

        #region Implementation of ISlot

        public void UpdateSlot(params object[] parameters)
        {
            Tick();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
        }

        #endregion
    }
}