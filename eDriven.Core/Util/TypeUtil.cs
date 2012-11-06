using System.Reflection;
using UnityEngine;

namespace eDriven.Core.Util
{
    public static class TypeUtil
    {
        public static readonly object[] Index = new object[] { };

        public static object GetValue(MemberInfo memberInfo, object target)
        {
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (null != fieldInfo)
            {
                //Debug.Log("Field");
                try
                {
                    return fieldInfo.GetValue(target);
                }
                catch
                {
                    Debug.Log(string.Format("Error getting value from [{0}]", fieldInfo.Name));
                }
            }

            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (null != propertyInfo)
            {
                //Debug.Log("Property");
                try
                {
                    return propertyInfo.GetValue(target, Index);
                }
                catch
                {
                    Debug.Log(string.Format("Error getting value from [{0}]", propertyInfo.Name));
                }
            }

            return null;
        }

        public static void SetValue(MemberInfo memberInfo, object target, object value)
        {
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (null != fieldInfo){
                try
                {
                    fieldInfo.SetValue(target, value);
                }
                catch
                {
                    Debug.Log(string.Format("Error setting value [{0}] to [{1}]", value, fieldInfo.Name));
                }
            }
                
            else
            {
                PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                if (null != propertyInfo)
                {
                    try
                    {
                        propertyInfo.SetValue(target, value, Index);
                    }
                    catch
                    {
                        Debug.Log(string.Format("Error setting value [{0}] to [{1}]", value, propertyInfo.Name));
                    }
                }
            }
        }
    }
}
