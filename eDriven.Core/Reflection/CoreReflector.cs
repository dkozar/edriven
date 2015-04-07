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
using System.Collections.Generic;
using System.Reflection;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Reflection utility
    /// </summary>
    public static class CoreReflector
    {
#if DEBUG
// ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode = true;

// ReSharper restore UnassignedField.Global
#endif

        private static FieldInfo[] _fieldInfos;
        private static PropertyInfo[] _propertyInfos;

        private static readonly object[] Index = { };

        /// <summary>
        /// Checks if the target contains member with the specified name
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="memberName">Field or property name</param>
        /// <returns></returns>
        public static bool HasMember(object target, string memberName)
        {
            Type targetType = target.GetType();

            var propertyInfo = targetType.GetProperty(memberName);
            if (null != propertyInfo)
            {
                return true;
            }
            var fieldInfo = targetType.GetField(memberName);
            if (null != fieldInfo)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the property value<br/>
        /// Uses internal caching
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="property">Property name</param>
        /// <returns></returns>
        public static object GetValue(object target, string property)
        {
            Type type = target.GetType();

            MemberWrapper memberWrapper = GlobalMemberCache.Instance.Get(type, property);

            if (null == memberWrapper) {
                memberWrapper = new MemberWrapper(type, property);
                GlobalMemberCache.Instance.Put(type, property, memberWrapper);
            }

            return memberWrapper.GetValue(target);
        }

        /// <summary>
        /// Sets the property value<br/>
        /// Uses internal caching
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="property">Property name</param>
        /// <param name="value">Value</param>
        public static void SetValue(object target, string property, object value)
        {
            //Debug.Log(string.Format(@"SetValue {0}, ""{1}"", ""{2}""", target, property, value));
            Type type = target.GetType();

            MemberWrapper memberWrapper = GlobalMemberCache.Instance.Get(type, property);

            if (null == memberWrapper)
            {
                memberWrapper = new MemberWrapper(type, property);
                GlobalMemberCache.Instance.Put(type, property, memberWrapper);
            }

            memberWrapper.SetValue(target, value);
        }

        /// <summary>
        /// Proxies an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static MemberProxy GetProxy(object item, string variable)
        {
            return new MemberProxy(item, variable);
        }

        /// <summary>
        /// Gets all field and propertiy names for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetFieldAndPropertyNames(Type type)
        {
            // TODO: we might cache this search
            List<string> names = new List<string>();

            _fieldInfos = type.GetFields();
            foreach (FieldInfo info in _fieldInfos)
            {
                names.Add(info.Name);
            }

            _propertyInfos = type.GetProperties();
            foreach (PropertyInfo info in _propertyInfos)
            {
                names.Add(info.Name);
            }

            names.Sort();

            return names;
        }

        /// <summary>
        /// Gets all members for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<MemberInfo> GetMembers(Type type)
        {
            return new List<MemberInfo>(type.GetMembers());
        }

        /// <summary>
        /// Gets a member value
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetMemberValue(MemberInfo memberInfo, object obj)
        {
            object value = null;

            FieldInfo fi = memberInfo as FieldInfo;
            if (null != fi)
            {
                value = fi.GetValue(obj);
                //Debug.Log(string.Format("  -> Field: {0}", memberInfo.Name));
            }
            else
            {
                PropertyInfo pi = memberInfo as PropertyInfo;
                if (null != pi)
                {
                    value = pi.GetValue(obj, Index);
                    //Debug.Log(string.Format("  -> Property: {0}", memberInfo.Name));
                }
            }

            return value;
        }

        /// <summary>
        /// Sets a member value
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetMemberValue(MemberInfo memberInfo, object obj, object value)
        {
            FieldInfo fi = memberInfo as FieldInfo;
            if (null != fi)
            {
                fi.SetValue(obj, value);
                //Debug.Log(string.Format("  -> Field: {0}", memberInfo.Name));
            }
            else
            {
                PropertyInfo pi = memberInfo as PropertyInfo;
                if (null != pi)
                {
                    pi.SetValue(obj, value, Index);
                    //Debug.Log(string.Format("  -> Property: {0}", memberInfo.Name));
                }
            }
        }

        #region Class attributes

        /// <summary>
        /// Gets all the attributes of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static List<T> GetClassAttributes<T>(Type type, bool inherit = true)
        {
            List<T> outputList = new List<T>();
            var attributes = type.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    outputList.Add((T)attribute);
                }
            }
            return outputList;
        }

        /// <summary>
        /// Gets all the attributes of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static bool HasClassAttributes<T>(Type type, bool inherit = true)
        {
            var attributes = type.GetCustomAttributes(typeof(T), inherit);
            return attributes.Length > 0;
        }

        #endregion

        #region Member attributes

        /// <summary>
        /// Gets all the attributes of the specified type
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static bool HasMemberAttributes<T>(MemberInfo memberInfo, bool inherit = true)
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.Length > 0;
        }

        /// <summary>
        /// Gets all the member attributes of the specified type
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetMemberAttributes<T>(MemberInfo memberInfo, bool inherit = true)
        {
            List<T> outputList = new List<T>();
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    outputList.Add((T)attribute);
                }
            }
            return outputList;
        }

        #endregion

        #region Method attributes

        /// <summary>
        /// Gets all the attributes of the specified type
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static bool HasMethodAttributes<T>(MethodInfo methodInfo, bool inherit = true)
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.Length > 0;
        }

        /// <summary>
        /// Gets all the method attributes of the specified type
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetMethodAttributes<T>(MethodInfo methodInfo, bool inherit = true)
        {
            List<T> outputList = new List<T>();
            var attributes = methodInfo.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    outputList.Add((T)attribute);
                }
            }
            return outputList;
        }

        #endregion
    }
}