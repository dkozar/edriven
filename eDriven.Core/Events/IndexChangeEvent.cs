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

namespace eDriven.Core.Events
{
    /// <summary>
    /// The event that holds the old and the new value
    /// </summary>
    public class IndexChangeEvent : Event
    {
// ReSharper disable InconsistentNaming

        /// <summary>
        /// Constant
        /// </summary>
        public const string SELECTED_INDEX_CHANGING = "selectedIndexChanging";

        /// <summary>
        /// Constant
        /// </summary>
        public const string SELECTED_INDEX_CHANGED = "selectedIndexChanged";
// ReSharper restore InconsistentNaming

        /// <summary>
        /// The zero-based index before the change.
        /// </summary>
        public int OldIndex;

        /// <summary>
        /// The zero-based index after the change
        /// </summary>
        public int Index;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Event type</param>
        public IndexChangeEvent(string type)
            : base(type)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Event type</param>
        /// <param name="target">Event target</param>
        public IndexChangeEvent(string type, object target)
            : base(type, target)
        {
        }
    }
}