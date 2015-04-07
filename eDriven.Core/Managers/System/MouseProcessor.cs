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

using eDriven.Core.Geom;
using eDriven.Core.Signals;
using eDriven.Core.Util;
using UnityEngine;
using Event=UnityEngine.Event;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Processes mouse events
    /// </summary>
    public class MouseProcessor : UnityEventProcessorBase
    {
#if DEBUG
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;
#endif
        private Point _mousePosition = new Point();

        /// <summary>
        /// When we make the mousedown action, the last mousedown event is saved here<br/>
        /// This is because the MouseEventDispatcher needs this info when building mousemove events<br/>
        /// Mouse move events need the button info because of handling component states
        /// </summary>
        public static Event MouseDownEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="systemManager"></param>
        public MouseProcessor(SystemManager systemManager)
        {
            SystemManager = systemManager;
        }

        /// <summary>
        /// Precessing the mouse
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Event e)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("MouseProcessor.Process");
#endif
            _mousePosition = new Point(e.mousePosition.x, e.mousePosition.y);

            //Debug.Log("MouseProcessor.Process: " + e.mousePosition);

            /**
             * Note: MouseMove events could be sent in editor only!!!
             * -> look into ScreenSizeProcessor class for capturing mouse move
             * */

            if (e.rawType == EventType.MouseDrag)
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log("MouseProcessor.MouseDrag");
#endif
                //SystemManager.MouseDragHandler(e, _mousePosition);

                if (SystemManager.MouseDragSignal.Connected)
                    SystemManager.MouseDragSignal.Emit(e, _mousePosition);

                //e.Use();
            }

            else if (e.rawType == EventType.MouseDown)
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log("MouseProcessor.MouseDown");
#endif
                Signal signal = MouseUtil.DifferentiateMouseButton(e.button,
                                                               SystemManager.MouseDownSignal,
                                                               SystemManager.RightMouseDownSignal,
                                                               SystemManager.MiddleMouseDownSignal
                );

                MouseDownEvent = e;

                if (signal.Connected)
                    signal.Emit(e, _mousePosition);

                //e.Use();
            }

            else if (e.rawType == EventType.MouseUp)
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log("MouseProcessor.MouseUp");
#endif
                Signal signal = MouseUtil.DifferentiateMouseButton(e.button,
                                                                   SystemManager.MouseUpSignal,
                                                                   SystemManager.RightMouseUpSignal,
                                                                   SystemManager.MiddleMouseUpSignal
                    );

                MouseDownEvent = null;

                if (signal.Connected)
                    signal.Emit(e, _mousePosition);
            }

            //e.Use();
        }

        /// <summary>
        /// A special method, since ScrollWheel event is not a mouse event nor a key event
        /// </summary>
        /// <param name="e"></param>
        public void ProcessWheel(Event e)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("MouseProcessor.ProcessWheel");
#endif
            _mousePosition = new Point(e.mousePosition.x, e.mousePosition.y);

#if DEBUG
            if (DebugMode)
                Debug.Log("MouseProcessor.ScrollWheel");
#endif
            if (SystemManager.MouseWheelSignal.Connected)
                SystemManager.MouseWheelSignal.Emit(e, _mousePosition, e.delta.y);

            e.Use(); // cancel the Unity default
        }
    }
}