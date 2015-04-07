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

namespace eDriven.Core.Events
{
    /// <summary>
    /// Keyboard event
    /// </summary>
    public class KeyboardEvent : InputEvent
    {
        #region Constants

// ReSharper disable InconsistentNaming
        /// <summary>
        /// Constant
        /// </summary>
        public const string KEY_DOWN = "keyDown";

        /// <summary>
        /// Constant
        /// </summary>
        public const string KEY_UP = "keyUp";
// ReSharper restore InconsistentNaming

        #endregion

        #region Properties

        /// <summary>
        /// Key code
        /// </summary>
        public KeyCode KeyCode;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardEvent(string type) : base(type)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardEvent(string type, object target) : base(type, target)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardEvent(string type, bool bubbles, bool cancelable)
            : base(type, bubbles, cancelable)
        {
        }

        #endregion

        public override string ToString()
        {
            //return string.Format("KeyCode: {0}, {1}", KeyCode, base.ToString());
            return string.Format("{0}{1}{2}{3}",
                KeyCode,
                Control ? " + Control" : string.Empty,
                Shift ? " + Shift" : string.Empty,
                Alt ? " + Alt" : string.Empty
            );
        }
    }
}