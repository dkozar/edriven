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
    /// Touch event
    /// This event bubbles by default
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class TouchEvent : Event
    {
        #region Constants

// ReSharper disable InconsistentNaming
        /// <summary>
        /// Constant
        /// </summary>
        public const string TOUCH = "touch";

        /// <summary>
        /// Constant
        /// </summary>
        public const string SINGLE_TOUCH = "singleTouch";

        /// <summary>
        /// Constant
        /// </summary>
        public const string DOUBLE_TOUCH = "doubleTouch";

        /// <summary>
        /// Constant
        /// </summary>
        public const string TRIPPLE_TOUCH = "trippleTouch";

        
        /// <summary>
        /// Constant
        /// </summary>
        public const string QUADRUPLE_TOUCH = "quadrupleTouch";

        
        /// <summary>
        /// Constant
        /// </summary>
        public const string QUINTUPLE_TOUCH = "quintupleTouch";

// ReSharper restore InconsistentNaming

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TouchEvent(string type) : base(type)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TouchEvent(string type, object target) : base(type, target)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TouchEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TouchEvent(string type, bool bubbles, bool cancelable)
            : base(type, bubbles, cancelable)
        {
        }

        #endregion

        public override string ToString()
        {
            return string.Format("TouchEvent [{0}]", Type);
        }
    }
}