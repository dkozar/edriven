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
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// The wrapper around the class member<br/>
    /// The main idea behind this approach is that whis wrapper could be cached<br/>
    /// and reused per class basis
    /// </summary>
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
        private static bool _found;

        /// <summary>
        /// A flag indicating this is a nullable type, so we'll need the conversion when setting the value
        /// </summary>
        private readonly Type _nullableType;
        
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
        /// <param name="type">Target type</param>
        /// <param name="name">Variable name</param>
        public MemberWrapper(Type type, string name)
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log(string.Format("Creating MemberWrapper [{0}, {1}]", type, name));
            }
#endif
            _found = false;

            // 1. properties
            _propertyInfo = type.GetProperty(name); //, /*BindingFlags.Instance | */BindingFlags.Public/* | BindingFlags.NonPublic*/);
            if (null != _propertyInfo)
            {
                _memberType = _propertyInfo.PropertyType;
                _found = true;
            }
            else
            {
                // 2.a. public fields, 2.b, private fields
                _fieldInfo = type.GetField(name) ?? type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
                if (null != _fieldInfo)
                {
                    _memberType = _fieldInfo.FieldType;
                    _isField = true;
                    _found = true;
                }
            }
            /*Debug.Log("* " + _memberType.Name + " (" + _memberType.FullName + ")");
            if (null == _memberType)
                throw new Exception("Couldn't reflect member: " + _memberType);*/

            if (null != _memberType)
                _nullableType = Nullable.GetUnderlyingType(_memberType);

            if (!_found)
                throw new MemberNotFoundException(string.Format(@"Couldn't find property nor field named ""{0}"" [{1}]", name, type));
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
                try
                {

                    if (null != _nullableType)
                    {
                        try
                        {
                            _fieldInfo.SetValue(
                                target,
                                null != value ? Convert.ChangeType(value, _nullableType) : null
                                );
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Cannot convert to nullable type", ex);
                        }
                    }
                    else
                    {
                        _fieldInfo.SetValue(target, value
                            /*, BindingFlags.Public | BindingFlags.NonPublic, null, CultureInfo.CurrentCulture*/);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot set value", ex);
                }
            }
            else
            {
#if DEBUG
                if (DebugMode)
                {
                    Debug.Log(string.Format("Setting PROPERTY value {0} for {1} on target {2}", value, _propertyInfo.Name, target));
                }
#endif
                if (null != _nullableType)
                {
                    try
                    {
                        _propertyInfo.SetValue(
                            target, 
                            null != value ? Convert.ChangeType(value, _nullableType) : null, 
                            null
                        );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Cannot convert to nullable type: " + ex);
                    } 
                }
                else
                {
                    _propertyInfo.SetValue(target, value, null);
                }
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

        /// <summary>
        /// Returns the member info
        /// </summary>
        public MemberInfo MemberInfo
        {
            get
            {
                if (_isField)
                {
                    return _fieldInfo;
                }

                return _propertyInfo;
            }
        }

        public override string ToString()
        {
            return string.Format("MemberType: {0}, IsField: {1}", _memberType, _isField);
        }
    }
}