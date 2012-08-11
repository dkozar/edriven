using System;
using System.Collections.Generic;
using System.Reflection;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Reflection utility
    /// </summary>
    public class ReflectionUtil
    {
        private static FieldInfo _fieldInfo;
        private static PropertyInfo _propertyInfo;

        private static FieldInfo[] _fieldInfos;
        private static PropertyInfo[] _propertyInfos;

        /// <summary>
        /// Gets the value of the property of an object
        /// </summary>
        /// <param name="item"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetValue(object item, string property)
        {
            Type type = item.GetType();

            _fieldInfo = type.GetField(property);

            // return value
            if (null != _fieldInfo)
                return _fieldInfo.GetValue(item);

            _propertyInfo = type.GetProperty(property);

            // return value
            if (null != _propertyInfo)
                return _propertyInfo.GetValue(item, null);

            throw new Exception(string.Format(@"Property or field ""{0}"" not found in object type ""{1}""", property, type.Name));
        }

        private static readonly object[] Arr = new object[] { };

        /// <summary>
        /// Sets the property of an object
        /// </summary>
        /// <param name="item"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetValue(object item, string property, object value)
        {
            //Debug.Log(string.Format(@"SetValue {0}, ""{1}"", ""{2}""", item, property, value));

            Type type = item.GetType();

            _fieldInfo = type.GetField(property);

            if (null != _fieldInfo)
                _fieldInfo.SetValue(item, value);

            else
            {
                _propertyInfo = type.GetProperty(property);

                if (null != _propertyInfo)
                    _propertyInfo.SetValue(item, value, Arr);
                else
                    throw new Exception(string.Format(@"Property or field ""{0}"" not found in object type ""{1}""", property, type.Name));
            }
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
        /// Gets all fields and properties for a type
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
    }
}