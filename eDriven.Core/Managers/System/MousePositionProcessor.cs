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
using eDriven.Core.Util;
using UnityEngine;
using Event=UnityEngine.Event;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Processes mouse position changes
    /// </summary>
    internal class MousePositionProcessor : UnityEventProcessorBase
    {
#if DEBUG
        public static bool DebugMode;
#endif

        private readonly Point _mousePosition = new Point();

        private Vector3 _rawMousePosition;
        private Vector3 _coords;

        public MousePositionProcessor(SystemManager systemManager)
        {
            SystemManager = systemManager;
        }

        /// <summary>
        /// Note: e is null because MouseMoveProcessor doesn't deal with Event.current! (not inside of the OnGUI)
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Event e)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("MousePositionProcessor.Process");
#endif
            Vector3 pos = Input.mousePosition;

            if (_rawMousePosition.x != pos.x || _rawMousePosition.y != pos.y)
            {
                _coords = StageUtil.FlipY(pos);

                _mousePosition.X = _coords.x;
                _mousePosition.Y = _coords.y;

#if DEBUG
                if (DebugMode)
                    //Debug.Log("Input.mousePosition: " + Input.mousePosition + "; _rawMousePosition: " + _rawMousePosition + "; GlobalPosition:" + _coords);
                    Debug.Log("mouse: GlobalPosition:" + _mousePosition);
#endif

                // propagate position to system manager
                SystemManager.MousePosition.X = _mousePosition.X;
                SystemManager.MousePosition.Y = _mousePosition.Y;

                _rawMousePosition = pos;
                //Debug.Log("    -> _rawMousePosition:" + _rawMousePosition);

                if (SystemManager.Instance.MouseMoveSignal.Connected)
                    SystemManager.Instance.MouseMoveSignal.Emit(null, _mousePosition.Clone());
            }
        }
    }
}