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

/*using System.Reflection;
using UnityEngine;

namespace eDriven.Core.Mono
{
    /// <summary>
    /// Maps unity event handlers to virtual methods
    /// </summary>
    public class SystemBehaviour : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// Should system manager be instantiated
        /// TRUE by default
        /// </summary>
        public static bool UseSystemManager = true;

        #endregion

        #region Unity handlers

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void Start()
// ReSharper restore UnusedMember.Local
        {
            OnStart();
        }

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void Awake()
// ReSharper restore UnusedMember.Local
        {
            OnAwake();
        }

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void Update()
// ReSharper restore UnusedMember.Local
        {
            OnUpdate();
        }

        // ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void LateUpdate()
        // ReSharper restore UnusedMember.Local
        {
            OnLateUpdate();
        }

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void FixedUpdate()
// ReSharper restore UnusedMember.Local
        {
            OnFixedUpdate();
        }

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
        [Obfuscation(Exclude = true)]
        void OnGUI()
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
        {
            Draw();
        }

        #endregion

        #region Virtual functions

        /// <summary>
        /// Runs on Awake
        /// </summary>
        public virtual void OnAwake()
        {

        }

        /// <summary>
        /// Runs on Start
        /// </summary>
        public virtual void OnStart()
        {

        }

        /// <summary>
        /// Runs on OnGUI
        /// </summary>
        public virtual void Draw()
        {

        }

        /// <summary>
        /// Runs on Update
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// Runs on FixedUpdate
        /// </summary>
        public virtual void OnFixedUpdate()
        {

        }

        /// <summary>
        /// Runs on LateUpdate
        /// </summary>
        public virtual void OnLateUpdate()
        {

        }

        #endregion

    }
}*/