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
    /// Event
    /// An event is used for transferring data around
    /// It also has methods for cancelling or stopping it's propagation
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class Event : ICloneable
    {
        public const string CHANGE = "change";

        #region Properties

        /// <summary>
        /// Event target (or originator)
        /// By default, the object that dispatched the event
        /// It may be set to any object (not necesarily the one that dispatched it)
        /// If not set explicitelly, the target is set by the system to default
        /// </summary>
        public object Target;

        /// <summary>
        /// The event type
        /// Used for notifiying listeners subscribed to specific event type
        /// </summary>
        public string Type;

        /// <summary>
        /// The flag that indicates if the event could be canceled or default prevented
        /// Using this flag, the developer can cancel/default prevent some behaviour inside the event dispatcher from the 'outside'
        /// </summary>
        public bool Cancelable;

        /// <summary>
        /// The flag that indicates if the event has been canceled
        /// Using this flag, the developer can cancel some default behaviour inside the event dispatcher from the 'outside'
        /// After the event has been canceled, it won't be dispatched to any of the consequent listeners
        /// </summary>
        public bool Canceled;

        /// <summary>
        /// The flag that indicates if the event has been default prevented
        /// Using this flag, the developer can prevent a default action of the event dispatcher
        /// This mechanism is used to expose a part of the decision making from event dispatcher to the 'outside'
        /// </summary>
        public bool DefaultPrevented;

        /// <summary>
        /// Current target
        /// Used by systems that support event bubbling
        /// to keep track of the current object processing the event
        /// </summary>
        public object CurrentTarget;

        /// <summary>
        /// The flag used by systems that support event bubbling
        /// false by default
        /// </summary>
        public bool Bubbles; // Changed from true to false on 2011-09-18

        /// <summary>
        /// Event phase (Capture/Target/Bubbling)
        /// Used by systems that support event bubbling
        /// to keep track of the current bubbling phase
        /// </summary>
        public EventPhase Phase = EventPhase.Target;

        #endregion

        #region Constructor

        /// <summary>
        /// Constant
        /// </summary>
        public Event(string type)
        {
            Type = type;
        }

        /// <summary>
        /// Constant
        /// </summary>
        public Event(string type, object target)
        {
            Type = type;
            Target = target;
        }

        /// <summary>
        /// Constant
        /// </summary>
        public Event(string type, bool bubbles)
        {
            Type = type;
            Bubbles = bubbles;
        }

        /// <summary>
        /// Constant
        /// </summary>
        public Event(string type, bool bubbles, bool cancelable)
        {
            Type = type;
            Cancelable = cancelable;
            Bubbles = bubbles;
        }

        /// <summary>
        /// Constant
        /// </summary>
        public Event(string type, object target, bool bubbles, bool cancelable)
        {
            Type = type;
            Target = target;
            Cancelable = cancelable;
            Bubbles = bubbles;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prevents the default action of the dispatcher following the dispatching
        /// </summary>
        public virtual void PreventDefault()
        {
            DefaultPrevented = true;
        }

        /// <summary>
        /// Cancels the event
        /// e.g. sets Canceled to TRUE
        /// </summary>
        public virtual void Cancel()
        {
            if (Cancelable)
                Canceled = true;
        }

        /// <summary>
        /// Stops the propagation
        /// Used by systems that support event bubbling
        /// Cancels the further bubbling but does not cancel the event
        /// </summary>
        public void StopPropagation()
        {
            Bubbles = false;
        }

        /// <summary>
        /// Stops the propagation
        /// Used by systems that support event bubbling
        /// Cancels the further bubbling AND cancels the event
        /// </summary>
        public void CancelAndStopPropagation()
        {
            Bubbles = false;
            Canceled = true;
        }

        /// <summary>
        /// Makes a shallow copy of the event
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Formatted string for debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[{0}] Target: {1}, Type: {2}, Canceled: {3}, CurrentTarget: {4}, Bubbles: {5}, EventPhase: {6}", GetType().FullName, Target, Type, Canceled, CurrentTarget, Bubbles, Phase);
        }

        #endregion
    }
}