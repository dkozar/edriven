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

using System.Collections.Generic;

namespace eDriven.Core.Signals
{
    /// <summary>
    /// Signal class
    /// Inspired by AS3 Signals
    /// </summary>
    /// <see cref="https://github.com/robertpenner/as3-signals/"/>
    public class Signal : ISignal
    {
        #region Members

        private readonly List<Slot> _slots = new List<Slot>();
        private readonly List<Slot> _autoDisconnectSlots = new List<Slot>();

        #endregion

        #region Implementation of IStrictSignal

        public int NumSlots
        {
            get { return _slots.Count; }
        }

        #region Connect overloads

        public void Connect(Slot slot, int priority, bool autoDisconnect)
        {
            if (_slots.Contains(slot))
                return; // do nothing
            
            _slots.Add(slot);
            if (autoDisconnect)
                _autoDisconnectSlots.Add(slot);
        }
        public void Connect(Slot slot, int priority)
        {
            Connect(slot, priority, false);
        }
        public void Connect(Slot slot)
        {
            Connect(slot, 50);
        }

        #endregion

        public bool Connected
        {
            get { return _slots.Count > 0; }
        }

        public bool Disconnect(Slot slot)
        {
            return _slots.Remove(slot);
        }

        public void Emit(params object[] parameters)
        {
            _slots.ForEach(delegate (Slot slot)
                               {
                                   slot(parameters);
                                  
                                   // disconect one-time receivers
                                   if (!_autoDisconnectSlots.Contains(slot)) 
                                       return;

                                   _slots.Remove(slot);
                                   _autoDisconnectSlots.Remove(slot);
                               });
        }

        public bool HasSlot(Slot slot)
        {
            return _slots.Contains(slot);
        }

        #endregion
    }
}