#region License

/*
 
Copyright (c) 2012 Danko Kozar

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

using UnityEngine;
using Event=UnityEngine.Event;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// Processes Unity keyboard events
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
    internal class KeyboardProcessor : UnityEventProcessorBase
    {
#if DEBUG
        public static bool DebugMode;
#endif

        public KeyboardProcessor(SystemManager systemManager)
        {
            SystemManager = systemManager;
        }
        
        public override void Process(Event e)
        {

#if DEBUG
            if (DebugMode)
                Debug.Log("KeyboardProcessor.Process");
#endif

            /**
             * 1) Overal key events
             * */
            if (e.type == EventType.KeyDown)
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log("KeyboardProcessor.KeyDown");
#endif
                if (SystemManager.KeyDownSignal.Connected)
                    SystemManager.KeyDownSignal.Emit(e);

            }

            else if (e.type == EventType.KeyUp)
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log("KeyboardProcessor.KeyUp");
#endif

                if (SystemManager.KeyUpSignal.Connected)
                    SystemManager.KeyUpSignal.Emit(e);
            }

            if (e.keyCode == KeyCode.Tab || e.character == '\t')
                e.Use();
        }
    }
}