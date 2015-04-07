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
using eDriven.Core.Signals;
using eDriven.Core.Util;
using UnityEngine;

namespace eDriven.Networking.External
{
    /// <summary>
    /// The Singleton which communicates with Javascript in browser host page
    /// Could be put anywhere in the hierarchy, because it handles it's own path when communicating with Javascript
    /// The path (to GameObject containing this script) is being registered with Javascript just before the first Javascript method call
    /// </summary>
    [AddComponentMenu("eDriven/Networking/JavascriptConnector")]
    public sealed class JavascriptConnector : MonoBehaviour
    {
#if DEBUG
// ReSharper disable UnassignedField.Global
        public static bool DebugMode;
// ReSharper restore UnassignedField.Global
#endif

        #region Singleton

        private static JavascriptConnector _instance;

        /// <summary>
        /// Singleton class for handling focus
        /// </summary>
        private JavascriptConnector()
        {
            // Constructor is protected
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static JavascriptConnector Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating JavascriptConnector instance"));
#endif
                    _instance = (JavascriptConnector) FindObjectOfType(typeof (JavascriptConnector));

                    JavascriptConnector[] instances = (JavascriptConnector[]) FindObjectsOfType(typeof (JavascriptConnector));

                    if (instances.Length == 0)
                    {
                        throw new Exception("No JavascriptConnector instance found");
                    }

                    if (instances.Length > 1)
                    {
                        throw new Exception("Multiple instances of JavascriptConnector found");
                    }

                    _instance = instances[0];
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the Singleton instance
        /// </summary>
        private void Initialize()
        {
            _callbacks = new Dictionary<int, ExternalCallCallback>();
        }

        /// <summary>
        /// Callback parameter separator
        /// </summary>
        public char CallbackStringSeparator = '|';
        /// <summary>
        /// A single string, integer or float argument must be passed when using SendMessage(), 
        /// the parameter is required on the calling side. 
        /// If you don't need it then just pass a zero or other default value and ignore it on the Unity side. 
        /// Additionally, the game object specified by the name can be given in the form of a path name. 
        /// For example, /MyObject/SomeChild where SomeChild must be a child of MyObject and MyObject 
        /// must be at the root level due to the '/' in front of its name. 
        /// </summary>
        /// <param name="value">The string value returned from browser Javascript</param>
// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void Callback(string value)
// ReSharper restore UnusedMember.Local
        {
            //_signal.Emit(param);
            string[] parts = value.Split(CallbackStringSeparator);

            if (parts.Length != 2)
                throw new Exception("parts length is not 2");

            int id;
            try
            {
                id = Convert.ToInt32(parts[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("parts[0] is not an integer: " + ex);
            }

            if (!_callbacks.ContainsKey(id))
                return; // no callback. The call had no callback defined.
            //throw new Exception(string.Format("callbacks dictionary doesn't contain id={0}", id));

            ExternalCallCallback callback = _callbacks[id];

            callback(parts[1]);

            _callbacks.Remove(id);

            if (_callbacks.Count == 0)
                _id = 0; // reset
        }

        private int _id;

        private Queue<ExternalCallParams> _queue; // = new Queue<ExternalCallParams>();

        private Dictionary<int, ExternalCallCallback> _callbacks;

        private static bool _registered;
        /// <summary>
        /// True if Javascript connector already registered
        /// </summary>
        public static bool Registered
        {
            get { return _registered; }
        }

        private bool _registerRequestSent;

        /// <summary>
        /// The name of the connector object
        /// </summary>
        public static string JavascriptConnectorObject = "eDriven.Connector";

        /// <summary>
        /// Registration method name
        /// </summary>
        public static string JavascriptRegistrationMethod = "registerConnector";

        /// <summary>
        /// Calls the Javascript method
        /// </summary>
        /// <param name="javascriptMethod">The name of the Javascript method</param>
        /// <param name="callback">Callback function</param>
        /// <param name="parameters">Javascript method parameters</param>
        public void Call(string javascriptMethod, ExternalCallCallback callback, params object[] parameters)
        {
            if (!_registered)
            {
                if (!_registerRequestSent)
                {
                    if (null == _queue)
                        _queue = new Queue<ExternalCallParams>();

                    //_queue.Enqueue(new ExternalCallParams(JavascriptRegistrationMethod, _id, new object[] { gameObject.name })); // gameObject.transform.path??
                    _callbacks[_id] = RegistrationCallback;

                    string path = GameObjectUtil.HierarchyAsString(gameObject);

#if DEBUG
                    if (DebugMode)
                    {
                        Debug.Log(string.Format(@"The path is ""{0}""", path));
                    }
#endif
                    Application.ExternalCall(string.Format("{0}.{1}", JavascriptConnectorObject, JavascriptRegistrationMethod), _id,
                                             new object[] { path, "Callback", "DeepLink" });

                    _registerRequestSent = true;
                }
            }

            _id++;

            _queue.Enqueue(new ExternalCallParams(javascriptMethod, _id, parameters));

            if (null != callback)
                _callbacks[_id] = callback;

            if (_registered || Application.isEditor)
            {
                ProcessQueue();
            }
        }

        [Obfuscation(Exclude = true)]
        private void RegistrationCallback(string value)
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log("Callback registered: " + value);
            }
#endif
            _registered = true;
            ProcessQueue();
        }

        private void ProcessQueue()
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log("Processing queue: Number of requests: " + _queue.Count);
            }
#endif
            while (_queue.Count > 0)
            {
                var p = _queue.Dequeue();
                Application.ExternalCall(string.Format("{0}.{1}", JavascriptConnectorObject, p.JavascriptMethod), p.Id, p.Parameters);
            }
        }

        public delegate void ExternalCallCallback(string value);

        private struct ExternalCallParams
        {
            public string JavascriptMethod;
            public int Id;
            public object[] Parameters;

            public ExternalCallParams(string javascriptMethod, int id, object[] parameters)
            {
                JavascriptMethod = javascriptMethod;
                Id = id;
                Parameters = parameters;
            }
        }

        public Signal DeepLinkSignal = new Signal();

// ReSharper disable UnusedMember.Local
        /// <summary>
        /// This method is being called from Javascript
        /// </summary>
        /// <param name="value"></param>
        void DeepLink(string value)
// ReSharper restore UnusedMember.Local
        {
            DeepLinkSignal.Emit(value);
        }
    }
}