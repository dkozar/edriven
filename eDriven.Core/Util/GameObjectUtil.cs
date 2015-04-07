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

using System.Collections.Generic;
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The utility for handling game objects
    /// </summary>
    public class GameObjectUtil
    {
        /// <summary>
        /// The signature of the method which handles a child game object
        /// </summary>
        /// <param name="child"></param>
        public delegate void ChildHandler(GameObject child);

        /// <summary>
        /// Iterates all children of a game object
        /// </summary>
        /// <param name="gameObject">A root game object</param>
        /// <param name="childHandler">A function to execute on each child</param>
        /// <param name="recursive">Do it on recursively on children?</param>
        public static void IterateChildren(GameObject gameObject, ChildHandler childHandler, bool recursive)
        {
            DoIterate(gameObject, childHandler, recursive);
        }

        /// <summary>
        /// Recursively iterates the object tree
        /// </summary>
        /// <param name="gameObject">Game object to iterate</param>
        /// <param name="childHandler">A handler function on node</param>
        /// <param name="recursive">Do it on recursively on children?</param>
        /// <remarks>NOTE: Recursive!!!</remarks>
        private static void DoIterate(GameObject gameObject, ChildHandler childHandler, bool recursive)
        {
            foreach (Transform child in gameObject.transform)
            {
                childHandler(child.gameObject);
                if (recursive)
                    DoIterate(child.gameObject, childHandler, true);
            }
        }

        /// <summary>
        /// Returns the hierarchy (path) of the supplied GameObject
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetHierarchy(GameObject gameObject)
        {
            List<GameObject> currentHierarchy = new List<GameObject>();

            GetHierarchyRecursive(gameObject, currentHierarchy);
            
            // reverse
            currentHierarchy.Reverse();

            return currentHierarchy;
        }

        /// <summary>
        /// RECURSIVE!!!
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="currentHierarchy"></param>
        /// <returns></returns>
        private static void GetHierarchyRecursive(GameObject gameObject, ICollection<GameObject> currentHierarchy)
        {
            currentHierarchy.Add(gameObject);

            if (null != gameObject.transform.parent) {
                var parent = gameObject.transform.parent.gameObject;
                GetHierarchyRecursive(parent, currentHierarchy);
            }
        }

        /// <summary>
        /// Returns the hierarchy (path) to the supplied GameObject
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Stringify(List<GameObject> hierarchy, string delimiter)
        {
            if (null == delimiter)
                delimiter = "/";

            List<string> names = new List<string>();
            hierarchy.ForEach(delegate (GameObject go)
                                  {
                                      names.Add(go.name);
                                  });
            return string.Join(delimiter, names.ToArray());
        }

        /// <summary>
        /// Returns a string representation of the object hierarchy
        /// </summary>
        /// <param name="gameObject">The object to get the hierarchy for</param>
        /// <param name="delimiter">Delimiter for a string concatenation</param>
        /// <returns></returns>
        public static string HierarchyAsString(GameObject gameObject, string delimiter)
        {
            List<GameObject> hierarchy = GetHierarchy(gameObject);
            return Stringify(hierarchy, delimiter);
        }

        /// <summary>
        /// Returns a string representation of the object hierarchy with the default delimiter ("/")
        /// </summary>
        /// <param name="gameObject">The object to get the hierarchy for</param>
        /// <returns></returns>
        public static string HierarchyAsString(GameObject gameObject)
        {
            List<GameObject> hierarchy = GetHierarchy(gameObject);
            return Stringify(hierarchy, "/");
        }
    }
}