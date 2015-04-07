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
using System.Reflection;
using System.Text;

namespace eDriven.Core.Signals
{
    /// <summary>
    /// Signal class
    /// eDriven signals are inspired by Robert Penner's signals (Actionscript 3) -> Signals are so inspiring, Rob! :)
    /// </summary>
    /// <see cref="https://github.com/robertpenner/as3-signals/"/>
    public class Signal : ISignal
    {
        #region Members

        private readonly List<Slot> _slots = new List<Slot>();
        private readonly List<Slot> _autoDisconnectSlots = new List<Slot>();

        #endregion

        #region Implementation of ISignal

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

            SlotCountChanged(_slots.Count);
        }

        public void Connect(Slot slot, bool autoDisconnect)
        {
            Connect(slot, 50, autoDisconnect);
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
            bool removed = _slots.Remove(slot);

            if (removed)
                SlotCountChanged(_slots.Count);

            return removed;
        }

        public void DisconnectAll()
        {
            _slots.Clear();
            SlotCountChanged(0);
        }

        private readonly List<Slot> _toRemove = new List<Slot>();
        public void Emit(params object[] parameters)
        {
            _slots.ForEach(delegate(Slot slot)
            {
                slot.Invoke(parameters);

                // disconect one-time receivers
                if (!_autoDisconnectSlots.Contains(slot))
                    return;

                _toRemove.Add(slot);
                _autoDisconnectSlots.Remove(slot);
            });

            _toRemove.ForEach(delegate(Slot slot)
            {
                //_slots.Remove(slot);
                Disconnect(slot);
            });
            _toRemove.Clear();
        }

        public bool HasSlot(Slot slot)
        {
            return _slots.Contains(slot);
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            _slots.ForEach(delegate (Slot slot)
                               {
                                   sb.AppendLine(DescribeSlot(slot));
                               });
            if (sb.ToString() == string.Empty)
                sb.Append("-none-");

            return string.Format(@"Slots ({0})
--------------------------------------------
{1}
--------------------------------------------", _slots.Count, sb);
        }

        /// <summary>
        /// Returns Signal type
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        public static string DescribeSignal(Signal signal)
        {
            return string.Format("Signal: {0}", signal.GetType());
        }

        /// <summary>
        /// Returns Slot metadata
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public string DescribeSlot(Slot slot)
        {
            bool isAuto = _autoDisconnectSlots.Contains(slot);
            MethodInfo mi = slot.Method;
            return string.Format("{0} -> {1}{2}", mi.ReflectedType, mi.Name, isAuto ? " [auto]" : string.Empty);
        }

        /// <summary>
        /// Use this method to add the additional functionality when needed to react on the subscriber change
        /// </summary>
// ReSharper disable UnusedParameter.Global
        protected virtual void SlotCountChanged(int count) { }
// ReSharper restore UnusedParameter.Global
    }
}