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

namespace eDriven.Core.Events
{
    /// <summary>
    /// The class that holds an instance of event dispatcher
    /// The instance is created lazily, i.e. the first time that event-dispatching functionality is needed
    /// This is a performance optimization
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class LazyEventDispatcher : IEventDispatcher
    {
        private EventDispatcher _dispatcher;

        private void Initialize()
        {
            if (_dispatcher == null)
            {
                _dispatcher = new EventDispatcher(this);
            }
        }

        #region Implementation of IEventDispatcher

        /// <summary>
        /// Dispatches an event
        /// </summary>
        /// <param name="e">Event to be dispatched</param>
        public void DispatchEvent(Event e)
        {
            Initialize();
            _dispatcher.DispatchEvent(e);
        }

        /// <summary>
        /// Dispatches an event with an option for switching on delayed processing
        /// </summary>
        /// <param name="e">Event</param>
        /// <param name="immediate">Process immediatelly or on the next update?</param>
        public void DispatchEvent(Event e, bool immediate)
        {
            Initialize();
            _dispatcher.DispatchEvent(e, immediate);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        public void AddEventListener(string eventType, EventHandler handler)
        {
            Initialize();
            _dispatcher.AddEventListener(eventType, handler);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="priority">Event priority</param>
        public void AddEventListener(string eventType, EventHandler handler, int priority)
        {
            Initialize();
            _dispatcher.AddEventListener(eventType, handler, priority);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        public void AddEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            Initialize();
            _dispatcher.AddEventListener(eventType, handler, phases);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        /// <param name="priority">Event priority</param>
        public void AddEventListener(string eventType, EventHandler handler, EventPhase phases, int priority)
        {
            Initialize();
            _dispatcher.AddEventListener(eventType, handler, phases, priority);
        }

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        public void RemoveEventListener(string eventType, EventHandler handler)
        {
            Initialize();
            _dispatcher.RemoveEventListener(eventType, handler);
        }

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases"></param>
        public void RemoveEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            Initialize();
            _dispatcher.RemoveEventListener(eventType, handler, phases);
        }

        /// <summary>
        /// Removes all event listeners of a certain type
        /// </summary>
        /// <param name="eventType">Event type</param>
        public void RemoveAllListeners(string eventType)
        {
            Initialize();
            _dispatcher.RemoveAllListeners(eventType);
        }

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific type of event. 
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool HasEventListener(string eventType)
        {
            Initialize();
            return _dispatcher.HasEventListener(eventType);
        }

        /// <summary>
        /// Checks whether an event listener is registered with this EventDispatcher object or any of its ancestors for the specified event type. 
        /// This method returns true if an event listener is triggered during any phase of the event flow when an event of the specified type is dispatched to this EventDispatcher object or any of its descendants.
        /// The difference between HasEventListener() and HasBubblingEventListener() is that HasEventListener() examines only the object to which it belongs, whereas HasBubblingEventListener() examines the entire event flow for the event specified by the type parameter.
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool HasBubblingEventListener(string eventType)
        {
            Initialize();
            return _dispatcher.HasBubblingEventListener(eventType);
        }

        #endregion
    }
}