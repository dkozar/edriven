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
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace eDriven.Core.Tasks
{
    /// <summary>
    /// Async task that could be synced via TaskQueue
    /// </summary>
    public abstract class TaskBase : ITask
    {
        /// <summary>
        /// Debug mode on
        /// </summary>
        public static bool DebugMode;

        #region Properties

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        private TaskQueue.Callback _callback;
        public TaskQueue.Callback Callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        private GameObject _parent;
        /// <summary>
        /// The parent game object
        /// </summary>
        public GameObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private string _description;
        /// <summary>
        /// The job description (will be displayed in progress bars)
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private Token _token = new Token();
        public virtual Token Token
        {
            get
            {
                return _token;
            }
            internal set
            {
                _token = value;
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public abstract bool IsDone { get; }

        private bool _excluded;
        /// <summary>
        /// A flag indicating that this job is included in progress
        /// </summary>
        public bool Excluded
        {
            get { return _excluded; }
        }
        
        #endregion
        
        #region Members

        private DateTime _startTime;

        #endregion

        #region Methods

        /// <summary>
        /// Heartbeat
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Runs a job
        /// </summary>
        public virtual void Run()
        {

            _startTime = DateTime.Now;

#if DEBUG
            if (DebugMode)
                //Debug.Log(string.Format(@"######### STARTING job [{0}] at {1} #########", this, _startTime.ToShortTimeString()));
                Debug.Log(string.Format(@"######### STARTING job [{0}] #########", this));
#endif

        }

        public override string ToString()
        {
            return string.Format(GetType().Name);
        }

        #endregion

        #region Chaining (fluent interface)

        /// <summary>
        /// Sets a job description (displayed in progress bar)
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public TaskBase SetDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Sets a parent GameObject
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TaskBase SetParent(GameObject parent)
        {
            Parent = parent;
            return this;
        }

        /// <summary>
        /// Sets the callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public TaskBase SetCallback(TaskQueue.Callback callback)
        {
            Callback = callback;
            return this;
        }

        /// <summary>
        /// Excludes a job from the list to show in progress bar
        /// </summary>
        /// <returns></returns>
        public TaskBase Exclude()
        {
            _excluded = true;
            return this;
        }

        #endregion
    }
}