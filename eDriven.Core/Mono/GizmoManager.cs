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

/*using System;
using System.Reflection;
using UnityEngine;
using Object=UnityEngine.Object;

namespace eDriven.Core.Mono
{
    /// <summary>
    /// Gizmo manager
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class GizmoManager : MonoBehaviour
    {
        #region Singleton

        private static GizmoManager _instance;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static GizmoManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Object[] retValue = FindObjectsOfType(typeof (GizmoManager));
                    if (retValue == null || retValue.Length == 0)
                        Framework.CreateComponent<GizmoManager>(true);
                    else if (retValue.Length > 1)
                        throw new ApplicationException("More than one GizmoManager object exists on the scene!");

// ReSharper disable PossibleNullReferenceException
                    _instance = (GizmoManager) retValue[0];
// ReSharper restore PossibleNullReferenceException
                }
                return _instance;
            }
        }

        protected GizmoManager()
        {
            // constructor is protected
        }

        #endregion

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void OnDrawGizmos()
// ReSharper restore UnusedMember.Local
        {
            //_gizmoManager.OnDrawGizmos();

            Gizmos.color = new Color(63.0f / 255.0f, 1.0f, 175.0f / 255.0f, 1.0f); //Color.green;

            //_targets.ForEach(delegate(GameObject go)
            //{
            //    Gizmos.DrawLine(transform.position, go.transform.position);
            //    Gizmos.DrawIcon(go.transform.position, "gizmo_listener.png");
            //});

            Gizmos.DrawIcon(transform.position, "gizmo_dispatcher.png");
        }
    }
}*/