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

using System.Collections.Generic;
using eDriven.Core.Events;
using UnityEngine;
using EventHandler=eDriven.Core.Events.EventHandler;
using MulticastDelegate=eDriven.Core.Events.MulticastDelegate;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Connects to SystemManager signals
    /// Dispatches touch events to interested parties
    /// </summary>
    public sealed class TouchEventDispatcher : EventDispatcher
    {

#if DEBUG
        /// <summary>
        /// Debug mode
        /// </summary>
        public new static bool DebugMode;
#endif
        
        #region Singleton

        private static TouchEventDispatcher _instance;

        private TouchEventDispatcher()
        {
            // Constructor is protected!
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static TouchEventDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating TouchEventDispatcher instance"));
#endif
                    _instance = new TouchEventDispatcher();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// A list of plugins
        /// </summary>
        private readonly List<ITouchEventDispatcherPlugin> _plugins = new List<ITouchEventDispatcherPlugin>();

        #region Initialization

        /// <summary>
        /// Initialization routine
        /// </summary>
        private void Initialize()
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format("Initializing TouchEventDispatcher"));
#endif
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the plugin
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(ITouchEventDispatcherPlugin plugin)
        {
            _plugins.Add(plugin);
            plugin.Initialize(this);
        }

        /// <summary>
        /// Removes the plugin
        /// </summary>
        /// <param name="plugin"></param>
        public void RemovePlugin(ITouchEventDispatcherPlugin plugin)
        {
            _plugins.Remove(plugin);
            plugin.Dispose();
        }

        #endregion

        #region Multicast delegates

        private MulticastDelegate _singleTouch;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate SingleTouch
        {
            get
            {
                if (null == _singleTouch)
                    _singleTouch = new MulticastDelegate(this, TouchEvent.SINGLE_TOUCH);
                return _singleTouch;
            }
            set
            {
                _singleTouch = value;
            }
        }

        private MulticastDelegate _doubleTouch;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate DoubleTouch
        {
            get
            {
                if (null == _doubleTouch)
                    _doubleTouch = new MulticastDelegate(this, TouchEvent.DOUBLE_TOUCH);
                return _doubleTouch;
            }
            set
            {
                _doubleTouch = value;
            }
        }

        private MulticastDelegate _trippleTouch;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate TrippleTouch
        {
            get
            {
                if (null == _trippleTouch)
                    _trippleTouch = new MulticastDelegate(this, TouchEvent.TRIPPLE_TOUCH);
                return _trippleTouch;
            }
            set
            {
                _trippleTouch = value;
            }
        }

        private MulticastDelegate _quadrupleTouch;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate QuadrupleTouch
        {
            get
            {
                if (null == _quadrupleTouch)
                    _quadrupleTouch = new MulticastDelegate(this, TouchEvent.QUADRUPLE_TOUCH);
                return _quadrupleTouch;
            }
            set
            {
                _quadrupleTouch = value;
            }
        }

        private MulticastDelegate _quintupleTouch;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate QuintupleTouch
        {
            get
            {
                if (null == _quintupleTouch)
                    _quintupleTouch = new MulticastDelegate(this, TouchEvent.QUINTUPLE_TOUCH);
                return _quintupleTouch;
            }
            set
            {
                _quintupleTouch = value;
            }
        }

        #endregion

        #region Handlers

        private int _touchCount;

        public override void AddEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            base.AddEventListener(eventType, handler, phases);

            _touchCount++;
            SystemManager.Instance.TouchSignal.Connect(TouchSlot);
        }

        public override void RemoveEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            base.RemoveEventListener(eventType, handler, phases);
            
            _touchCount--;
            if (_touchCount <= 0)
            {
                SystemManager.Instance.TouchSignal.Disconnect(TouchSlot);
            }
        }

        #endregion

        #region Slots

        private void TouchSlot(params object[] parameters)
        {
            if (_touchCount <= 0)
                return;

            Touch[] touches = (Touch[])parameters[0];

#if DEBUG
            if (DebugMode)
            {
                Debug.Log("Touch with " + touches.Length + " fingers");
            }
#endif
            
            TouchEvent touchEvent = new TouchEvent(TouchEvent.TOUCH);
            DispatchEvent(touchEvent);

            if (touchEvent.Canceled)
                return;

            switch (touches.Length)
            {
                case 1:
                    touchEvent = new TouchEvent(TouchEvent.SINGLE_TOUCH);
                    DispatchEvent(touchEvent);
                    break;
                case 2:
                    touchEvent = new TouchEvent(TouchEvent.DOUBLE_TOUCH);
                    DispatchEvent(touchEvent);
                    break;
                case 3:
                    touchEvent = new TouchEvent(TouchEvent.TRIPPLE_TOUCH);
                    DispatchEvent(touchEvent);
                    break;
                case 4:
                    touchEvent = new TouchEvent(TouchEvent.QUADRUPLE_TOUCH);
                    DispatchEvent(touchEvent); 
                    break;
                case 5:
                    touchEvent = new TouchEvent(TouchEvent.QUINTUPLE_TOUCH);
                    DispatchEvent(touchEvent);
                    break;
                default:
                    break;
            }

            // process custom gestures
            foreach (ITouchEventDispatcherPlugin plugin in _plugins)
            {
                plugin.Process(touches);
            }
        }

        #endregion

        #region IDisposable

        public override void Dispose()
        {
            base.Dispose();

            foreach (ITouchEventDispatcherPlugin plugin in _plugins)
            {
                plugin.Dispose();
            }
            _plugins.Clear();
        }

        #endregion

    }
}