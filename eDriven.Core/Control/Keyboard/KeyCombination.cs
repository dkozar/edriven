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
using UnityEngine;

namespace eDriven.Core.Control.Keyboard
{
    /// <summary>
    /// The class representing the key combination
    /// (KeyCode + Control + Shift + Alt)
    /// </summary>
    public class KeyCombination
    {
        #region Properties

        /// <summary>
        /// The event type (key up/key down)
        /// </summary>
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public string EventType;

        /// <summary>
        /// Key code
        /// </summary>
        public KeyCode KeyCode;

        /// <summary>
        /// Control key on
        /// </summary>
        public bool Control;

        /// <summary>
        /// Shift key on
        /// </summary>
        public bool Shift;

        /// <summary>
        /// Alt key on
        /// </summary>
        public bool Alt;
// ReSharper restore FieldCanBeMadeReadOnly.Global

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="keyCode"></param>
        /// <param name="control"></param>
        /// <param name="shift"></param>
        /// <param name="alt"></param>
        public KeyCombination(string eventType, KeyCode keyCode, bool control, bool shift, bool alt)
        {
            EventType = eventType;
            KeyCode = keyCode;
            Control = control;
            Alt = alt;
            Shift = shift;
        }

        #endregion

        #region Equals

        /// <summary>
        /// Equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(KeyCombination other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.EventType, EventType) && 
                Equals(other.KeyCode, KeyCode) && 
                other.Control.Equals(Control) && 
                other.Shift.Equals(Shift) && 
                other.Alt.Equals(Alt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (KeyCombination)) return false;
            return Equals((KeyCombination) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (EventType != null ? EventType.GetHashCode() : 0);
                result = (result*397) ^ KeyCode.GetHashCode();
                result = (result*397) ^ Control.GetHashCode();
                result = (result*397) ^ Shift.GetHashCode();
                result = (result*397) ^ Alt.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Creates the combo from keyboard event
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static KeyCombination FromKeyboardEvent(KeyboardEvent @event)
        {
            return new KeyCombination(@event.Type, @event.KeyCode, @event.Control, @event.Shift, @event.Alt);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{3}", 
                KeyCode,
                Control ? " + Control" : string.Empty,
                Shift ? " + Shift" : string.Empty,
                Alt ? " + Alt" : string.Empty
            );
        }
    }
}