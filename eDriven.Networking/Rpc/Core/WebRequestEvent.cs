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

using System;
using UnityEngine;

namespace eDriven.Networking.Rpc
{
    /// <summary>
    /// Web request event
    /// </summary>
    public class WebRequestEvent : Core.Events.Event, IDisposable
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        public const string EVENT_FINISHED = "finished";
        public const string EVENT_PROGRESS = "progress";
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global

        public WWW Request;

        public WebRequestEvent(string type) : base(type)
        {
        }

        public WebRequestEvent(string type, object target)
            : base(type, target)
        {
        }

        public WebRequestEvent(string type, bool bubbles)
            : base(type, bubbles)
        {
        }

        public WebRequestEvent(string type, bool bubbles, bool cancelable)
            : base(type, bubbles, cancelable)
        {
        }

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //if (null != Request)
            //{
            //    //if (null != Request.audioClip)
            //    //    UnityEngine.Object.Destroy(Request.audioClip);

            //    //if (null != Request.movie)
            //    //    UnityEngine.Object.Destroy(Request.movie);

            //    //if (Type == EVENT_FINISHED && null != Request.texture)
            //    //    UnityEngine.Object.Destroy(Request.texture);
            //}

            //Request = null;
        }

        #endregion
    }
}