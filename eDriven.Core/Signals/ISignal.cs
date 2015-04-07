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

namespace eDriven.Core.Signals
{
    /// <summary>
    /// The ability to emit signals
    /// </summary>
    public interface ISignal
    {
        /// <summary>
        /// Returns the number of connected slots
        /// </summary>
        int NumSlots { get; }

        /// <summary>
        /// Connects a slot to this signal
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="priority"></param>
        /// <param name="autoDisconnect"></param>
        void Connect(Slot slot, int priority, bool autoDisconnect);

        /// <summary>
        /// Connects a slot to this signal
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="priority"></param>
        void Connect(Slot slot, int priority);

        /// <summary>
        /// Connects a slot to this signal
        /// </summary>
        /// <param name="slot"></param>
        void Connect(Slot slot);

        /// <summary>
        /// Returns true if this signal has any connected slots
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Disconnects a slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        bool Disconnect(Slot slot);

        /// <summary>
        /// Disconnects a slot
        /// </summary>
        /// <returns></returns>
        void DisconnectAll();

        /// <summary>
        /// Emits the signal to connected slots
        /// </summary>
        /// <param name="parameters"></param>
        void Emit(params object[] parameters);

        /// <summary>
        /// Returns true if the specified slot is connected to this signal
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        bool HasSlot(Slot slot);
    }
}