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

namespace eDriven.Networking.Rpc
{
    public class HttpServiceProgressEvent : Event
    {
        /// <summary>
        /// 'PROGRESS_CHANGE' event notifies observes if something has moved between collections:
        /// Send() --> queue
        /// queue --> active
        /// active --> finished
        /// Useful for progress bars etc.
        /// NOTE: They doesn't necessarily fire as much times as there are requests
        /// </summary>
// ReSharper disable InconsistentNaming
        public static string PROGRESS_CHANGE = "progressChange";
// ReSharper restore InconsistentNaming

        public int Queued
        {
            get
            {
                return Total - Active - Finished;
            }
        }

        public int Active;
        public int Finished;

        public int Total;

        public HttpServiceProgressEvent(string type) : base(type)
        {
        }

        public HttpServiceProgressEvent(string type, object target) : base(type, target)
        {
        }

        public HttpServiceProgressEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        public HttpServiceProgressEvent(string type, bool bubbles, bool cancelable) : base(type, bubbles, cancelable)
        {
        }

        public override string ToString()
        {
            return string.Format(@"
 [{0} queued; {1} active; {2} finished; {3} total]", Queued, Active, Finished, Total);
        }
    }
}