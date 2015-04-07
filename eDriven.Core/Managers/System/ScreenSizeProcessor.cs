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
using UnityEngine;
using Event=UnityEngine.Event;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Processes screen size changes
    /// </summary>
    internal class ScreenSizeProcessor : UnityEventProcessorBase
    {
#if DEBUG
        public static bool DebugMode;
#endif
        private readonly Point _screenSize = new Point();

        public ScreenSizeProcessor(SystemManager systemManager)
        {
            SystemManager = systemManager;
        }

        public override void Process(Event e)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("ScreenSizeProcessor.Process");
#endif

            if (_screenSize.X != Screen.width || _screenSize.Y != Screen.height)
            {
                // screen dimensions have been changed
#if DEBUG
                if (DebugMode)
                    Debug.Log(string.Format("Screen resized from: [{0}, {1}] to: [{2}, {3}]", _screenSize.X,
                                            _screenSize.Y, Screen.width, Screen.height));
#endif
                _screenSize.X = Screen.width;
                _screenSize.Y = Screen.height;

                // set screen size to system manager
                SystemManager.ScreenSize = _screenSize;

                SystemManager.ResizeSignal.Emit(_screenSize.Clone());
            }
        }
    }
}