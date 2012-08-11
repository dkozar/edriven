using System;
using System.Reflection;
using UnityEngine;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// The wrapper around the class member</br>
    /// The main idea behind this approach is that whis wrapper could be cached<br/>
    /// and reused per class basis
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
    public class MemberWrapper
    {
#if DEBUG
// ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;

// ReSharper restore UnassignedField.Global
#endif

        private readonly bool _isField;
        private readonly FieldInfo _fieldInfo;
        private readonly PropertyInfo _propertyInfo;
        private readonly bool _found;
        
        private readonly Type _memberType;
        /// <summary>
        /// Member type resolved by this wrapper
        /// </summary>
        public Type MemberType
        {
            get
            {
                return _memberType;
            }
        }

        /// <summary>
        /// Constructor (empty)
        /// Because of type params
        /// </summary>
        public MemberWrapper()
        {
            // do nothing
            throw new Exception("Do not use this constructor");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public MemberWrapper(Type type, string name)
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log(string.Format("Creating MemberWrapper [{0}, {1}]", type, name));
            }
#endif
            _found = false;

            _propertyInfo = type.GetProperty(name);
            if (null != _propertyInfo)
            {
                _memberType = _propertyInfo.PropertyType;
                _found = true;
            }
            else
            {
                _fieldInfo = type.GetField(name);
                if (null != _fieldInfo)
                {
                    _memberType = _fieldInfo.FieldType;
                    _isField = true;
                    _found = true;
                }
            }

            if (!_found)
                throw new Exception(string.Format(@"Couldn't find property nor field named ""{0}""", name));
#if DEBUG
            if (DebugMode)
            {
                Debug.Log(string.Format("      -> Created: {0}, {1} [{2}]", type, name, _isField));
            }
#endif
        }

        /// <summary>
        /// Sets value on specified target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue(object target, object value)
        {
            if (_isField)
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log(string.Format("Setting FIELD value {0} for {1} on target {2}", value, _propertyInfo.Name, target));
                }
#endif
                _fieldInfo.SetValue(target, value);
            }
            else
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log(string.Format("Setting PROPERTY value {0} for {1} on target {2}", value, _propertyInfo.Name, target));
                }
#endif
                _propertyInfo.SetValue(target, value, null);
            }
        }

        /// <summary>
        /// Gets value from specified target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public object GetValue(object target)
        {
            if (_isField)
            {
                return _fieldInfo.GetValue(target);
            }
            
            return _propertyInfo.GetValue(target, null);
        }
    }
}