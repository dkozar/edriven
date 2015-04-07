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

namespace eDriven.Core.Serialization
{
    /// <summary>
    /// The collection indexable by string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringIndexedList<T> : List<T> where T : IUnique
    {
        /// <summary>
        /// Should I report errors
        /// </summary>
        public static bool ReportErrors = true;

        /// <summary>
        /// Gets the list item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T this[string id]
        {
            get
            {
                T item = FindItemById(id);

// ReSharper disable CompareNonConstrainedGenericWithNull
                if (null == item && ReportErrors)
// ReSharper restore CompareNonConstrainedGenericWithNull
                    throw new Exception(string.Format(@"StringIndexedList: Key ""{0}"" not found in the collection", id));

                return item;
            }
        }

        /// <summary>
        /// Finds a list item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private T FindItemById(string id)
        {
            T item = Find(delegate(T s)
            {
                return id == s.Id;
            });

            return item;
        }

        /// <summary>
        /// Checks if the colection contains the supplied ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(string id)
        {
// ReSharper disable CompareNonConstrainedGenericWithNull
            return null != FindItemById(id);
// ReSharper restore CompareNonConstrainedGenericWithNull
        }
    }
}