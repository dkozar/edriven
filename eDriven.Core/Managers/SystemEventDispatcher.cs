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

using eDriven.Core.Events;
using eDriven.Core.Geom;
using UnityEngine;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Connects to SystemManager signals
    /// Dispatches eDriven.Events to interested parties
    /// </summary>
    public sealed class SystemEventDispatcher : EventDispatcher
    {

#if DEBUG
        /// <summary>
        /// Debug mode
        /// </summary>
        public new static bool DebugMode;
#endif
        
        #region Singleton

        private static SystemEventDispatcher _instance;
        
        private SystemEventDispatcher()
        {
            // Constructor is protected!
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static SystemEventDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating SystemEventDispatcher instance"));
#endif
                    _instance = new SystemEventDispatcher();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialization routine
        /// </summary>
        private void Initialize()
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format("Initializing SystemEventDispatcher"));
#endif
            SystemManager.Instance.UpdateSignal.Connect(UpdateSlot);
        }

        #endregion

        #region Slots

        private void UpdateSlot(params object[] parameters)
        {
            // process queued events on SystemEventDispatcher
            // (those events came from KeyUpSlot, KeyDownSlot etc.
            ProcessQueue();
        }

        private void MouseMoveSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MOUSE_MOVE)
                                {
                                    //CurrentEvent = (UnityEngine.Event)parameters[0], // note: CurrentEvent is null
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MouseDragSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MOUSE_DRAG)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MouseDownSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MOUSE_DOWN)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MouseUpSlot(params object[] parameters)
        {
            //Debug.Log("Mouse up! ");
            MouseEvent me = new MouseEvent(MouseEvent.MOUSE_UP)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void RightMouseDownSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.RIGHT_MOUSE_DOWN)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void RightMouseUpSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.RIGHT_MOUSE_UP)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MiddleMouseDownSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MIDDLE_MOUSE_DOWN)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MiddleMouseUpSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MIDDLE_MOUSE_UP)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void MouseWheelSlot(params object[] parameters)
        {
            MouseEvent me = new MouseEvent(MouseEvent.MOUSE_WHEEL)
                                {
                                    CurrentEvent = (UnityEngine.Event)parameters[0],
                                    GlobalPosition = (Point)parameters[1]
                                };
            DispatchEvent(me);
        }

        private void KeyDownSlot(params object[] parameters)
        {
            UnityEngine.Event e = (UnityEngine.Event)parameters[0];
            //Debug.Log("KeyDownSlot: " + e.keyCode);

            KeyboardEvent ke = new KeyboardEvent(KeyboardEvent.KEY_DOWN)
                                   {
                                       CurrentEvent = e,
                                       KeyCode = e.keyCode,
                                       Shift = e.shift,
                                       Control = e.control,
                                       Alt = e.alt
                                   };
            DispatchEvent(ke, false);
        }

        private void KeyUpSlot(params object[] parameters)
        {
            UnityEngine.Event e = (UnityEngine.Event)parameters[0];

            KeyboardEvent ke = new KeyboardEvent(KeyboardEvent.KEY_UP)
                                   {
                                       CurrentEvent = e,
                                       KeyCode = e.keyCode,
                                       Shift = e.shift,
                                       Control = e.control,
                                       Alt = e.alt
                                   };
            DispatchEvent(ke, false);
        }

        #endregion

        #region Multicast delegates

        /// <summary>
        /// The event that fires when the left mouse button is down
        ///</summary>
        private MulticastDelegate _mouseDown;
        /// <summary>
        /// Fires left mouse down events
        /// </summary>
        public MulticastDelegate MouseDown
        {
            get
            {
                if (null == _mouseDown)
                    _mouseDown = new MulticastDelegate(this, MouseEvent.MOUSE_DOWN);
                return _mouseDown;
            }
            set
            {
                _mouseDown = value;
            }
        }

        private MulticastDelegate _mouseUp;
        /// <summary>
        /// Fires left mouse up events
        /// </summary>
        public MulticastDelegate MouseUp
        {
            get
            {
                if (null == _mouseUp)
                    _mouseUp = new MulticastDelegate(this, MouseEvent.MOUSE_UP);
                return _mouseUp;
            }
            set
            {
                _mouseUp = value;
            }
        }

        private MulticastDelegate _middleMouseDown;
        /// <summary>
        /// Fires middle mouse down events
        /// </summary>
        public MulticastDelegate MiddleMouseDown
        {
            get
            {
                if (null == _middleMouseDown)
                    _middleMouseDown = new MulticastDelegate(this, MouseEvent.MIDDLE_MOUSE_DOWN);
                return _middleMouseDown;
            }
            set
            {
                _middleMouseDown = value;
            }
        }

        private MulticastDelegate _middleMouseUp;
        /// <summary>
        /// Fires middle mouse up events
        /// </summary>
        public MulticastDelegate MiddleMouseUp
        {
            get
            {
                if (null == _middleMouseUp)
                    _middleMouseUp = new MulticastDelegate(this, MouseEvent.MIDDLE_MOUSE_UP);
                return _middleMouseUp;
            }
            set
            {
                _middleMouseUp = value;
            }
        }

        private MulticastDelegate _rightMouseDown;
        /// <summary>
        /// Fires right mouse down events
        /// </summary>
        public MulticastDelegate RightMouseDown
        {
            get
            {
                if (null == _rightMouseDown)
                    _rightMouseDown = new MulticastDelegate(this, MouseEvent.RIGHT_MOUSE_DOWN);
                return _rightMouseDown;
            }
            set
            {
                _rightMouseDown = value;
            }
        }

        private MulticastDelegate _rightMouseUp;
        /// <summary>
        /// Fires middle mouse up events
        /// </summary>
        public MulticastDelegate RightMouseUp
        {
            get
            {
                if (null == _rightMouseUp)
                    _rightMouseUp = new MulticastDelegate(this, MouseEvent.RIGHT_MOUSE_UP);
                return _rightMouseUp;
            }
            set
            {
                _rightMouseUp = value;
            }
        }

        private MulticastDelegate _mouseMove;
        /// <summary>
        /// Fires mouse move events
        /// </summary>
        public MulticastDelegate MouseMove
        {
            get
            {
                if (null == _mouseMove)
                    _mouseMove = new MulticastDelegate(this, MouseEvent.MOUSE_MOVE);
                return _mouseMove;
            }
            set
            {
                _mouseMove = value;
            }
        }

        private MulticastDelegate _mouseDrag;
        /// <summary>
        /// Fires mouse drag events
        /// </summary>
        public MulticastDelegate MouseDrag
        {
            get
            {
                if (null == _mouseDrag)
                    _mouseDrag = new MulticastDelegate(this, MouseEvent.MOUSE_DRAG);
                return _mouseDrag;
            }
            set
            {
                _mouseDrag = value;
            }
        }

        private MulticastDelegate _mouseWheel;
        /// <summary>
        /// Fires mouse wheel events
        /// </summary>
        public MulticastDelegate MouseWheel
        {
            get
            {
                if (null == _mouseWheel)
                    _mouseWheel = new MulticastDelegate(this, MouseEvent.MOUSE_WHEEL);
                return _mouseWheel;
            }
            set
            {
                _mouseWheel = value;
            }
        }

        private MulticastDelegate _keyDown;
        /// <summary>
        /// Fires key down events
        /// </summary>
        public MulticastDelegate KeyDown
        {
            get
            {
                if (null == _keyDown)
                    _keyDown = new MulticastDelegate(this, KeyboardEvent.KEY_DOWN);
                return _keyDown;
            }
            set
            {
                _keyDown = value;
            }
        }

        private MulticastDelegate _keyUp;
        /// <summary>
        /// Fires key up events
        /// </summary>
        public MulticastDelegate KeyUp
        {
            get
            {
                if (null == _keyUp)
                    _keyUp = new MulticastDelegate(this, KeyboardEvent.KEY_UP);
                return _keyUp;
            }
            set
            {
                _keyUp = value;
            }
        }

        private MulticastDelegate _resize;
        /// <summary>
        /// Fires resize events
        /// </summary>
        public MulticastDelegate Resize
        {
            get
            {
                if (null == _resize)
                    _resize = new MulticastDelegate(this, ResizeEvent.RESIZE);
                return _resize;
            }
            set
            {
                _resize = value;
            }
        }

        #endregion

        #region Handlers

        public override void AddEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            base.AddEventListener(eventType, handler, phases);

            switch (eventType)
            {
                // mouse
                case MouseEvent.MOUSE_MOVE:
                    //Debug.Log("Subscribing to MOUSE_MOVE: " + handler);
                    SystemManager.Instance.MouseMoveSignal.Connect(MouseMoveSlot);
                    break;
                case MouseEvent.MOUSE_DOWN:
                    SystemManager.Instance.MouseDownSignal.Connect(MouseDownSlot);
                    break;
                case MouseEvent.MOUSE_UP:
                    //Debug.Log("Subscribing to MOUSE_UP: " + handler);
                    SystemManager.Instance.MouseUpSignal.Connect(MouseUpSlot);
                    break;
                case MouseEvent.RIGHT_MOUSE_DOWN:
                    SystemManager.Instance.RightMouseDownSignal.Connect(RightMouseDownSlot);
                    break;
                case MouseEvent.RIGHT_MOUSE_UP:
                    SystemManager.Instance.RightMouseUpSignal.Connect(RightMouseUpSlot);
                    break;
                case MouseEvent.MIDDLE_MOUSE_DOWN:
                    SystemManager.Instance.MiddleMouseDownSignal.Connect(MiddleMouseDownSlot);
                    break;
                case MouseEvent.MIDDLE_MOUSE_UP:
                    SystemManager.Instance.MiddleMouseUpSignal.Connect(MiddleMouseUpSlot);
                    break;
                case MouseEvent.MOUSE_DRAG:
                    SystemManager.Instance.MouseDragSignal.Connect(MouseDragSlot);
                    break;
                case MouseEvent.MOUSE_WHEEL:
                    SystemManager.Instance.MouseWheelSignal.Connect(MouseWheelSlot);
                    break;

                // keys
                case KeyboardEvent.KEY_DOWN:
                    SystemManager.Instance.KeyDownSignal.Connect(KeyDownSlot);
                    break;
                case KeyboardEvent.KEY_UP:
                    SystemManager.Instance.KeyUpSignal.Connect(KeyUpSlot);
                    break;
            }
        }

        public override void RemoveEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            base.RemoveEventListener(eventType, handler, phases);

            switch (eventType)
            {
                // mouse
                case MouseEvent.MOUSE_MOVE:
                    //Debug.Log("Unsubscribing from MOUSE_MOVE: " + handler);
                    if (!HasEventListener(MouseEvent.MOUSE_MOVE))
                        SystemManager.Instance.MouseMoveSignal.Disconnect(MouseMoveSlot);
                    break;
                case MouseEvent.MOUSE_DOWN:
                    if (!HasEventListener(MouseEvent.MOUSE_DOWN))
                        SystemManager.Instance.MouseDownSignal.Disconnect(MouseDownSlot);
                    break;
                case MouseEvent.MOUSE_UP:
                    //Debug.Log("Unsubscribing from MOUSE_UP: " + handler);
                    if (!HasEventListener(MouseEvent.MOUSE_UP))
                    {
                        //Debug.Log("Unsubscribing from MOUSE_UP: " + handler);
                        SystemManager.Instance.MouseUpSignal.Disconnect(MouseUpSlot);
                    }
                    break;
                case MouseEvent.RIGHT_MOUSE_DOWN:
                    if (!HasEventListener(MouseEvent.RIGHT_MOUSE_DOWN))
                        SystemManager.Instance.RightMouseDownSignal.Disconnect(RightMouseDownSlot);
                    break;
                case MouseEvent.RIGHT_MOUSE_UP:
                    if (!HasEventListener(MouseEvent.RIGHT_MOUSE_UP))
                        SystemManager.Instance.RightMouseUpSignal.Disconnect(RightMouseUpSlot);
                    break;
                case MouseEvent.MIDDLE_MOUSE_DOWN:
                    if (!HasEventListener(MouseEvent.MIDDLE_MOUSE_DOWN))
                        SystemManager.Instance.MiddleMouseDownSignal.Disconnect(MiddleMouseDownSlot);
                    break;
                case MouseEvent.MIDDLE_MOUSE_UP:
                    if (!HasEventListener(MouseEvent.MIDDLE_MOUSE_UP))
                        SystemManager.Instance.MiddleMouseUpSignal.Disconnect(MiddleMouseUpSlot);
                    break;
                case MouseEvent.MOUSE_DRAG:
                    if (!HasEventListener(MouseEvent.MOUSE_DRAG))
                        SystemManager.Instance.MouseDragSignal.Disconnect(MouseDragSlot);
                    break;
                case MouseEvent.MOUSE_WHEEL:
                    if (!HasEventListener(MouseEvent.MOUSE_WHEEL))
                        SystemManager.Instance.MouseWheelSignal.Disconnect(MouseWheelSlot);
                    break;

                // keys
                case KeyboardEvent.KEY_DOWN:
                    if (!HasEventListener(KeyboardEvent.KEY_DOWN))
                        SystemManager.Instance.KeyDownSignal.Disconnect(KeyDownSlot);
                    break;
                case KeyboardEvent.KEY_UP:
                    if (!HasEventListener(KeyboardEvent.KEY_UP))
                        SystemManager.Instance.KeyUpSignal.Disconnect(KeyUpSlot);
                    break;
            }
        }

        #endregion

    }
}