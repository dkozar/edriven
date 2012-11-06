#region License

/*
 
Copyright (c) 2012 Danko Kozar

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

using System.Reflection;
using eDriven.Core.Managers;
using UnityEngine;

namespace eDriven.Core.Mono
{
    /// <summary>
    /// Invokes vital functions of SystemManager<br/>
    /// Since Unity doesn't offer the "master clock" functionality, we have to steal it from one of the GameObjects<br/>
    /// This component is created lazily by SystemManager itself the first time it is being referenced
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
    [Obfuscation(Exclude = true)]
    public sealed class SystemManagerInvoker : MonoBehaviour
    {
        /// <summary>
        /// Reference to system manager instance
        /// </summary>
        private SystemManager _systemManager;

        [Obfuscation(Exclude = true)]
        void Awake()
        {
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
            useGUILayout = false;
#endif
        }

        [Obfuscation(Exclude = true)]
        void Start()
        {
            _systemManager = SystemManager.Instance;
        }

        [Obfuscation(Exclude = true)]
        void Update()
        {
            _systemManager.ProcessUpdate();
        }

        [Obfuscation(Exclude = true)]
        void FixedUpdate()
        {
            _systemManager.ProcessFixedUpdate();
        }

        [Obfuscation(Exclude = true)]
        void LateUpdate()
        {
            _systemManager.ProcessLateUpdate();
        }

// ReSharper disable InconsistentNaming
        [Obfuscation(Exclude = true)]
        void OnGUI()
// ReSharper restore InconsistentNaming
        {
            _systemManager.ProcessInput();
        }

        [Obfuscation(Exclude = true)]
        void OnEnable()
        {
            _systemManager = SystemManager.Instance;
        }

        [Obfuscation(Exclude = true)]
        void OnDisable()
        {
            if (null != _systemManager)
            {
                if (_systemManager.SceneChangeSignal.Connected)
                    _systemManager.SceneChangeSignal.Emit();
            }
        }

        [Obfuscation(Exclude = true)]
        void OnLevelWasLoaded()
        {
            if (_systemManager.LevelLoadedSignal.Connected)
                _systemManager.LevelLoadedSignal.Emit();
        }
    }
}