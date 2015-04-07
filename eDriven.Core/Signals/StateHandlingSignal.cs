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

using UnityEngine;

namespace eDriven.Core.Signals
{
    /// <summary>
    /// The signal handling its state changes
    /// You could supply the OnSignalStateChanged callback function for getting the state changes
    /// The signal is processing the number of connected slots and firing the callback if the number of slots changes between 0 and 1
    /// </summary>
    public class StateHandlingSignal : Signal
    {
#if DEBUG
// ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;

// ReSharper restore UnassignedField.Global
#endif
        /// <summary>
        /// The signature of a callback that has to be set to this signal when wanting to receive state changes
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="connected"></param>
        public delegate void SignalStateChangedHandler(Signal signal, bool connected);

        private bool _connected;

        protected override void SlotCountChanged(int count)
        {
            if (count > 0 && !_connected)
            {
                _connected = true;
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log(string.Format(@"Signal connected: " + this));
                }
#endif
                if (null != OnSignalStateChanged)
                    OnSignalStateChanged(this, _connected);
            }
            else if (count == 0 && _connected)
            {
                _connected = false;
                
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log(string.Format(@"Signal disconnected: " + this));
                }
#endif
                if (null != OnSignalStateChanged)
                    OnSignalStateChanged(this, _connected);
            }
        }

        /// <summary>
        /// The callback processing the signal state change (i.e. has the Signal any subscribers)
        /// </summary>
        public SignalStateChangedHandler OnSignalStateChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onSignalStateChanged"></param>
        public StateHandlingSignal(SignalStateChangedHandler onSignalStateChanged)
        {
            OnSignalStateChanged = onSignalStateChanged;
        }
    }
}
