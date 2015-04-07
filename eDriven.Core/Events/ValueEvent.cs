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
    /// The event that contains a single value
    /// </summary>
    public class ValueEvent : Event
    {
        #region Constants

// ReSharper disable InconsistentNaming
        /// <summary>
        /// Value changed constant
        /// </summary>
        public static string VALUE_CHANGED = "ValueChanged";
// ReSharper restore InconsistentNaming

        #endregion

        /// <summary>
        /// Value
        /// </summary>
        public object Value;

        /// <summary>
        /// Index of value element for lists
        /// </summary>
        public int Index;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEvent(string type) : base(type)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEvent(string type, object target) : base(type, target)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEvent(string type, bool bubbles, bool cancelable) : base(type, bubbles, cancelable)
        {
        }

        public override string ToString()
        {
            return string.Format("ValueEvent, Index: {0}, Value: {1}", Index, Value);
        }
    }
}