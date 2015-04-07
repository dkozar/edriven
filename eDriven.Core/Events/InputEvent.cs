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

namespace eDriven.Core.Events
{
    /// <summary>
    /// The event that wrapps up UnityEngine.Event
    /// Has a reference to UnityEngine.Event.current as 
    /// It copies Shift, Control and Alt from UnityEngine.Event.current
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class InputEvent : Event
    {
        #region Properties

        /// <summary>
        /// Is Shift pressed
        /// </summary>
        public bool Shift;

        /// <summary>
        /// Is Control pressed
        /// </summary>
        public bool Control;

        /// <summary>
        /// Is Alt pressed
        /// </summary>
        public bool Alt;

        /// <summary>
        /// Current Unity event
        /// </summary>
        public UnityEngine.Event CurrentEvent
        {
            get
            {
                return _currentEvent;
            }
            set
            {
                _currentEvent = value;
                if (null != _currentEvent)
                {
                    Shift = _currentEvent.shift;
                    Control = _currentEvent.control;
                    Alt = _currentEvent.alt;
                }
            }
        }

        #endregion

        #region Members

        private UnityEngine.Event _currentEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public InputEvent(string type) : base(type)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InputEvent(string type, object target) : base(type, target)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InputEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InputEvent(string type, bool bubbles, bool cancelable) : base(type, bubbles, cancelable)
        {
        }

        #endregion

        #region Methods

        override public void Cancel()
        {
            base.Cancel();

            // canceles the UnityEngine.Event
            if (null != CurrentEvent)
                CurrentEvent.Use();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Control: {0}, Shift: {1}, Alt: {2}", Control, Shift, Alt);
        }
    }
}