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
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("Error setting value [{0}] to [{1}]", value, fieldInfo.Name));
                    Debug.LogError(ex);
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
