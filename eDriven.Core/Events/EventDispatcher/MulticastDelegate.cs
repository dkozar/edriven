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
using System.Collections.Generic;

namespace eDriven.Core.Events
{
    /// <summary>
    /// Used by subscribers of an event dispatcher for adding and removing event listeners using the += and -= notation<br/>
    /// This class maps '+=' to AddEventListener and '-=' to RemoveEventListener methods<br/>
    /// Beside that, it doesn't bring any new functionality<br/>
    /// Should be instantiated by event dispatcher using the composition<br/>
    /// Lazy instantiation should be considered for performance optimization<br/>
    /// Note: has overloaded += and -= operators
    /// </summary>
    public class MulticastDelegate : List<EventHandler> , IDisposable
    {
        /// <summary>
        /// The event type for this list
        /// </summary>
        private readonly string _eventType;
        /// <summary>
        /// Event type (read only)
        /// </summary>
        public string EventType
        {
            get { return _eventType; }
        }

        /// <summary>
        /// The event dispatcher to which this list has been applied to
        /// </summary>
        private readonly IEventDispatcher _dispatcher;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MulticastDelegate(IEventDispatcher dispatcher, string eventType)
        {
            _dispatcher = dispatcher;
            _eventType = eventType;
        }

        #endregion

        #region Editor entries

        /// <summary>
        /// Editor entry
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void AddListener(EventHandler handler)
        {
            _dispatcher.AddEventListener(_eventType, handler, EventPhase.Target | EventPhase.Bubbling); // could not be used for capture phase
        }

        /// <summary>
        /// Editor entry
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public void RemoveListener(EventHandler handler)
        {
            _dispatcher.RemoveEventListener(_eventType, handler, EventPhase.Target | EventPhase.Bubbling); // could not be used for capture phase
        }

        #endregion

        #region Operator overloads

        /// <summary>
        /// Overloading operator +=
        /// </summary>
        /// <param name="del"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static MulticastDelegate operator +(MulticastDelegate del, EventHandler handler)
        {
            del._dispatcher.AddEventListener(del._eventType, handler, EventPhase.Target | EventPhase.Bubbling); // could not be used for capture phase
            return del;
        }

        /// <summary>
        /// Overloading operator -=
        /// </summary>
        /// <param name="del"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static MulticastDelegate operator -(MulticastDelegate del, EventHandler handler)
        {
            del._dispatcher.RemoveEventListener(del._eventType, handler, EventPhase.Target | EventPhase.Bubbling); // could not be used for capture phase
            return del;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (null != _dispatcher)
                _dispatcher.RemoveAllListeners(_eventType);
        }

        #endregion
    }
}