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
    ///<summary>
    /// The ability for dispatching events
    ///</summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Dispatches an event
        /// </summary>
        /// <param name="e">Event to be dispatched</param>
        void DispatchEvent(Event e);
        
        /// <summary>
        /// Dispatches an event with an option for switching on delayed processing
        /// </summary>
        /// <param name="e">Event</param>
        /// <param name="immediate">Process immediatelly or on the next update?</param>
        void DispatchEvent(Event e, bool immediate);

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        void AddEventListener(string eventType, EventHandler handler);

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="priority">Event priority</param>
        void AddEventListener(string eventType, EventHandler handler, int priority);

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        void AddEventListener(string eventType, EventHandler handler, EventPhase phases);

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        /// <param name="priority">Event priority</param>
        void AddEventListener(string eventType, EventHandler handler, EventPhase phases, int priority);

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        void RemoveEventListener(string eventType, EventHandler handler);

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases"></param>
        void RemoveEventListener(string eventType, EventHandler handler, EventPhase phases);

        /// <summary>
        /// Removes all event listeners of a certain type
        /// </summary>
        /// <param name="eventType">Event type</param>
        void RemoveAllListeners(string eventType);

        /// <summary>
        /// Checks whether the EventDispatcher has any listeners registered for a specific type of event
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        bool HasEventListener(string eventType);

        /// <summary>
        /// Checks whether an event listener is registered with this EventDispatcher or any of its ancestors for the specified event type
        /// Note: the implementation of event bubbling depends of the use-case and is not implemented by eDriven.Core
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        bool HasBubblingEventListener(string eventType);
    }
}