using System;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Sets the specified value on a given object
    /// </summary>
    public interface ISetterProxy
    {
        /// <summary>
        /// Member type, which will be evaluated for assigning the right interpolator
        /// </summary>
        Type MemberType { get; set; }

        /// <summary>
        /// Sets a member value
        /// </summary>
        /// <param name="value"></param>
        void SetValue(object value);
    }
}