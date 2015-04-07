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
    /// Used by an EventDispatcher dictionary as a key
    /// </summary>
    public struct EventTypePhase // : IEquatable<EventTypePhase>
    {
        #region Properties

        /// <summary>
        /// Event type
        /// </summary>
        public string EventType;

        /// <summary>
        /// Event phase
        /// </summary>
        public EventPhase Phase;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EventTypePhase(string type)
        {
            EventType = type;
            Phase = EventPhase.Target | EventPhase.Bubbling; // this is the default
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EventTypePhase(string type, EventPhase phase)
        {
            EventType = type;
            Phase = phase;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return string.Format("Type: {0}, UseCapture: {1}", EventType, Phase);
        }

        #endregion

        #region Equals

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        /// </param>
        public bool Equals(EventTypePhase other)
        {
            return Equals(other.EventType, EventType) && Equals(other.Phase, Phase);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        /// </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (EventTypePhase)) return false;
            return Equals((EventTypePhase) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((EventType != null ? EventType.GetHashCode() : 0)*397) ^ Phase.GetHashCode();
            }
        }

        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(EventTypePhase left, EventTypePhase right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EventTypePhase left, EventTypePhase right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}