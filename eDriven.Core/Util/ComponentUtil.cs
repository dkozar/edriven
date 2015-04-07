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
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The utility for handling component references
    /// </summary>
    public class ComponentUtil
    {
        /// <summary>
        /// References the component of a particular type on a game object, which is looked up by name
        /// </summary>
        /// <typeparam name="T">Script type</typeparam>
        /// <param name="gameObjectName">Game object name</param>
        /// <returns></returns>
        public static T ReferenceScript<T>(string gameObjectName) where T : Component
        {
            // find cube
            GameObject go = GameObject.Find(gameObjectName);

            if (null == go)
                throw new Exception(string.Format("Cannot locate game object [{0}]", gameObjectName));

            // reference dispatcher
            T script = go.GetComponent<T>();

            if (null == go)
                throw new Exception(string.Format(@"Cannot locate script of type [{0}] on game object [{1}]", typeof(T), gameObjectName));

            return script;
        }
    }
}
