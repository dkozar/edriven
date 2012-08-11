using System;
using eDriven.Core.Util;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Class used for proxying a member via type + string member name
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
    public class MemberProxy
    {
        private static Cache<VariableTypeCombo, MemberWrapper> SetterCache
        {
            get
            {
                if (UseGlobalMemberCache)
                    return GlobalMemberTypeCache.Instance;

                return new Cache<VariableTypeCombo, MemberWrapper>();
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
        private object _target;
        private string _variable;
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

        private static readonly VariableTypeCombo Combo = new VariableTypeCombo();
        private MemberWrapper _setter;

        private void Initialize()
        {
            if (null == _target)
                throw new Exception("Target cannot be null");

            if (string.IsNullOrEmpty(_variable))
                throw new Exception("Variable not set");

            _type = _target.GetType();

            // get it from cache
            Combo.Type = _type;
            Combo.Variable = _variable;

            if (DoCacheMembers)
            {
                _setter = SetterCache.Get(Combo);

                if (null == _setter) // if nothing cached yet
                {
                    // cache it
                    _setter = new MemberWrapper(_type, _variable);
                    SetterCache.Put(Combo, _setter);
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
        /// </summary>
        public Type MemberType
        {
            get
            {
                return _memberType;
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