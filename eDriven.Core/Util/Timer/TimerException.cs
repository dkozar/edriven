using System;
using System.Runtime.Serialization;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The exception that can be thrown by ImageRotatorException
    /// </summary>
    public class TimerException : Exception
    {
        public static string DelayError = "Delay should be greater than 0";
        //public static string RepeatCountError = "RepeatCount should be greater or equal to 0";

        public TimerException()
        {
        }

        public TimerException(string message) : base(message)
        {
        }

        public TimerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TimerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}