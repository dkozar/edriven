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
using eDriven.Core.Caching;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Class used for proxying a member via type + string member name
    /// </summary>
    public class MemberProxy : ISetterProxy
    {
        private static MemberCache SetterCache
        {
            get
            {
                if (UseGlobalMemberCache)
                    return GlobalMemberCache.Instance;

                return new MemberCache();
            }
        }

        /// <summary>
        /// True if member wrappers be cached per class + member
        /// </summary>
        public static bool DoCacheMembers = true;

        /// <summary>
        /// True for using the global member cache
        /// </summary>
        public static bool UseGlobalMemberCache = true;

        private static Type _type;
        private readonly object _target;
        private readonly string _variable;
        private bool _initialized;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="variable"></param>
        public MemberProxy(object target, string variable)
        {
            _target = target;
            _variable = variable;
            Initialize();
        }

        private MemberWrapper _setter;

        private void Initialize()
        {
            if (null == _target)
                throw new Exception("Target cannot be null");

            if (string.IsNullOrEmpty(_variable))
                throw new Exception("Variable not set");

            _type = _target.GetType();

            if (DoCacheMembers)
            {
                _setter = SetterCache.Get(_type, _variable);

                if (null == _setter) // if nothing cached yet
                {
                    // cache it
                    _setter = new MemberWrapper(_type, _variable);
                    SetterCache.Put(_type, _variable, _setter);
                }
            }
            else
            {
                _setter = new MemberWrapper(_type, _variable);
            }

            _memberType = _setter.MemberType;

            _initialized = true;
        }

        private Type _memberType;
        /// <summary>
        /// Member type resolved by this proxy
        /// Used for evaludating the right interpolator
        /// </summary>
        public Type MemberType
        {
            get
            {
                return _memberType;
            }
            set
            {
                _memberType = value;
            }
        }

        /// <summary>
        /// Gets member value
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (!_initialized)
                throw new Exception(@"Proxy not initialized");

            return _setter.GetValue(_target);
        }

        /// <summary>
        /// Sets member value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            if (!_initialized)
                throw new Exception(@"Proxy not initialized");

            _setter.SetValue(_target, value);
        }
    }
}