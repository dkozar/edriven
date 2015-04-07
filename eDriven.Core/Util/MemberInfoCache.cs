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
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The class holding relations between types and their member types as MemberInfos
    /// </summary>
    public class MemberInfoCache
    {
#if DEBUG 
// ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;
// ReSharper restore UnassignedField.Global
#endif

        private readonly Dictionary<Type, List<MemberInfo>> _dict = new Dictionary<Type, List<MemberInfo>>();

        /// <summary>
        /// Gets the cached value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<MemberInfo> Get(Type key)
        {
            if (_dict.ContainsKey(key))
                return _dict[key];

            return null;
        }

        /// <summary>
        /// Caches a MemberInfo
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(Type key, MemberInfo value)
        {
            if (_dict.ContainsKey(key))
            {
                if (_dict[key].Contains(value))
                    return;
            }
            else
            {
#if DEBUG
	            if (DebugMode)
	            {
		            Debug.Log("MemberInfoCache: added type " + key.Name);
	            }
#endif
                _dict[key] = new List<MemberInfo>();
            }

            _dict[key].Add(value);
#if DEBUG
	        if (DebugMode)
	        {
		        Debug.Log("    -> added value " + value.Name);
	        }
#endif
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            foreach (List<MemberInfo> value in _dict.Values)
            {
                value.Clear();
            }
            _dict.Clear();
        }

        /// <summary>
        /// Gets count
        /// </summary>
        public int Count(Type key)
        {
            if (!_dict.ContainsKey(key))
                return 0;

            return _dict[key].Count;
        }
    }
}