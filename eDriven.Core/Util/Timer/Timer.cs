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
using eDriven.Core.Events;
using eDriven.Core.Managers;
using UnityEngine;
using Event=eDriven.Core.Events.Event;
using MulticastDelegate=eDriven.Core.Events.MulticastDelegate;

namespace eDriven.Core.Util
{
    /// <summary>
    /// Timer class
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class Timer : EventDispatcher
    {
#if DEBUG
        public new static bool DebugMode;
#endif

        #region Events

        // ReSharper disable InconsistentNaming
        public const string START = "start";
        public const string PAUSE = "pause";
        public const string STOP = "stop";
        public const string RESET = "reset";
        public const string TICK = "tick";
        public const string COMPLETE = "complete";
        // ReSharper restore InconsistentNaming

        private MulticastDelegate _startHandler;
        /// <summary>
        /// The handler which fires when the timer is started
        /// </summary>
        public MulticastDelegate StartHandler
        {
            get
            {
                if (null == _startHandler)
                    _startHandler = new MulticastDelegate(this, START);
                return _startHandler;
            }
            set
            {
                _startHandler = value;
            }
        }

        private MulticastDelegate _stopHandler;
        /// <summary>
        /// The handler which fires when the timer is stopped
        /// </summary>
        public MulticastDelegate StopHandler
        {
            get
            {
                if (null == _stopHandler)
                    _stopHandler = new MulticastDelegate(this, STOP);
                return _stopHandler;
            }
            set
            {
                _stopHandler = value;
            }
        }

        private MulticastDelegate _resetHandler;
        /// <summary>
        /// The handler which fires when the timer has been reset
        /// </summary>
        public MulticastDelegate ResetHandler
        {
            get
            {
                if (null == _resetHandler)
                    _resetHandler = new MulticastDelegate(this, RESET);
                return _resetHandler;
            }
            set
            {
                _resetHandler = value;
            }
        }

        private MulticastDelegate _tickHandler;
        /// <summary>
        /// The handler which fires on each timer tick
        /// </summary>
        public MulticastDelegate Tick
        {
            get
            {
                if (null == _tickHandler)
                    _tickHandler = new MulticastDelegate(this, TICK);
                return _tickHandler;
            }
            set
            {
                _tickHandler = value;
            }
        }

        private MulticastDelegate _completeHandler;
        /// <summary>
        /// The handler which fires when the timer is complete (on the last tick)
        /// </summary>
        public MulticastDelegate Complete
        {
            get
            {
                if (null == _completeHandler)
                    _completeHandler = new MulticastDelegate(this, COMPLETE);
                return _completeHandler;
            }
            set
            {
                _completeHandler = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Should the timer tick on start, or after the first delay
        /// </summary>
        public bool TickOnStart;

        private float _delay = 1f;
        /// <summary>
        /// Delay time in seconds
        /// (seconds because Unity deals with seconds rather than milliseconds)
        /// </summary>
        public float Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                if (value != _delay)
                {
                    if (_delay <= 0)
                        throw new TimerException(TimerException.DelayError);

                    _delay = value;

                    // if messing with delay, we should reset the timer!!! :)
                    // if not, strange effects happen (in calculating tick in the OnUpdate method, line "if (_time > _delay * _count) // step ")
                    Reset();
                }
            }
        }

        private float _repeatCount;
        /// <summary>
        /// Tick in seconds
        /// </summary>
        public float RepeatCount
        {
            get
            {
                return _repeatCount;
            }
            set
            {
                if (value != _repeatCount)
                {
                    if (_repeatCount < 0)
                        throw new TimerException(TimerException.DelayError);

                    _repeatCount = value;
                }

                CheckRepeatCount();
            }
        }

        private DateTime _lastTickTime;
        /// <summary>
        /// The time the timer had the last tick
        /// </summary>
        public DateTime LastTickTime
        {
            get
            {
                return _lastTickTime;
            }
        }

        /// <summary>
        /// The flag indicating if the timer is running
        /// </summary>
        public bool IsRunning { get; private set; }

        #endregion

        #region Members

        private float _time;
        private int _count;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Timer()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="delay"></param>
        public Timer(float delay) : this()
        {
            _delay = delay;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="repeatCount"></param>
        public Timer(float delay, int repeatCount) : this(delay)
        {
            _repeatCount = repeatCount;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            base.Dispose();

            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            Start(false);
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="reset">Should the time be reset</param>
        public void Start(bool reset)
        {
            if (!IsRunning)
                SystemManager.Instance.UpdateSignal.Connect(UpdateSlot); //, 0, false); // Subscribe to update signal
            
            IsRunning = true;

            if (reset)
                Reset();

            // dispatch start event
            if (HasEventListener(START))
                DispatchEvent(new Event(START));

            // if tick on start, dispatch tick event
            if (TickOnStart && !_paused)
                DoTick();

            _paused = false;
        }

        private bool _paused;

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Pause()
        {
            // Unsubscribe from update signal
            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);

            IsRunning = false;
            _paused = true;

            // dispatch stop event
            if (HasEventListener(PAUSE))
                DispatchEvent(new Event(PAUSE));
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            // Unsubscribe from update signal
            SystemManager.Instance.UpdateSignal.Disconnect(UpdateSlot);

            IsRunning = false;

            Reset();

            // dispatch stop event
            if (HasEventListener(STOP))
                DispatchEvent(new Event(STOP));
        }

        /// <summary>
        /// Resets time and the tick count
        /// </summary>
        public void Reset()
        {
            //Active = false;
            /**
            * Unsubscribe from SystemManager Update
            * */
            _time = 0;
            _count = 0;
            if (HasEventListener(RESET))
                DispatchEvent(new Event(RESET));
        }

        /// <summary>
        /// Resets time, but not the tick count
        /// </summary>
        public void Defer()
        {
            _time = 0;
        }

        #endregion

        #region Private methods

        private void DoTick()
        {
            _lastTickTime = DateTime.Now;
            if (HasEventListener(TICK))
                DispatchEvent(new Event(TICK));
        }

        private void CheckRepeatCount()
        {
            if (_repeatCount > 0 && _count >= _repeatCount){
                Stop();
                if (HasEventListener(COMPLETE))
                    DispatchEvent(new Event(COMPLETE));
            }
        }

        #endregion

        #region Implementation of ISlot

        /// <summary>
        /// The slot which is being executed on each update
        /// </summary>
        /// <param name="parameters"></param>
        public void UpdateSlot(params object[] parameters)
        {
            _time += Time.deltaTime;

            // the logic: maintain delay * count greater than time
            if (_time > _delay * (_count + 1)) // step
            {
                _count++;

#if DEBUG
                if (DebugMode)
                    Debug.Log("Timer tick.");
#endif
                DoTick();

                CheckRepeatCount();
            }
        }

        #endregion
    }
}