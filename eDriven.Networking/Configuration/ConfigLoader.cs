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
using eDriven.Networking.Rpc;
using UnityEngine;
using Object=UnityEngine.Object;

namespace eDriven.Networking.Configuration
{
    /// <summary>
    /// Configuration loader
    /// Only one instance of this object is needed in the application
    /// You should set AsyncMode and ConfigPath/ConfigUrl
    /// After that you don't work with it directly
    /// but with Configuration.Instance.Application, which is loaded by ConfigLoader
    /// You should subscribe to Configuration.INITIALIZED event which indicates that configuration is built
    /// and then start working with it
    /// </summary>
    /// 
    [AddComponentMenu("eDriven/Networking/ConfigLoader")]

    public class ConfigLoader : MonoBehaviour
    {
        #region Singleton

        private static ConfigLoader _instance;
        public static ConfigLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    Object[] retValue = FindObjectsOfType(typeof(ConfigLoader));
                    if (retValue == null || retValue.Length == 0)
                        throw new ApplicationException("ConfigLoader object doesn't exist in scene!");
                    if (retValue.Length > 1)
                        throw new ApplicationException("More than one ConfigLoader object exists on the scene!");

                    _instance = (ConfigLoader)retValue[0];
                }
                return _instance;
            }
        }

        protected ConfigLoader()
        {
            // constructor is protected
        }

//// ReSharper disable MemberCanBeMadeStatic.Global
//        protected Type ConfigurationObjectType
//// ReSharper restore MemberCanBeMadeStatic.Global
//        {
//            set
//            {
//                Configuration.Instance.ConfigurationObjectType = value;
//            }
//        }

        #endregion

        // ReSharper disable UnassignedField.Global
        public static bool DebugMode;
        // ReSharper restore UnassignedField.Global

        /// <summary>
        /// Loader can load config.xml in 2 fashions:
        /// 1) async mode, e.g. loading config from URL
        /// 2) sync mode, e.g. loading from Resources
        /// </summary>
        public bool AsyncMode = true;

        /// <summary>
        /// Should I never load from cache
        /// </summary>
        public bool CacheBuster = true;

        /// <summary>
        /// Used if AsyncMode is false
        /// The path in the Resources forder (without extension)
        /// </summary>
        public string ConfigPath = "Config/config";

        /// <summary>
        /// Used if AsyncMode is true
        /// Full config URL (including the ".xml" extension)
        /// </summary>
        public string ConfigUrl;

        /// <summary>
        /// Used if AsyncMode is true in Editor
        /// Full config URL (including the ".xml" extension)
        /// </summary>
        public string EditorConfigUrl;

        public static FaultHandler FaultHandler;

        public static ResultHandler ResultHandler;

        //private bool _shouldLoadOnUpdate;
        //private bool _configLoadingStarted;

        public virtual void Load()
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("Loading config...");
#endif
            //if (UnityEngine.Application.isEditor)
            //    StartLoading(); // start loading on the first update (and only when in editor)
            //else
            //    _shouldLoadOnUpdate = true;

            StartLoading();
            //_shouldLoadOnUpdate = true;
        }

        private HttpConnector _connector;

        Object _config;

        private void StartLoading()
        {
            Configuration.Instance.Deserilized = false;
            //Configuration.Instance.Initialized = false;

#if DEBUG
            if (DebugMode)
                Debug.Log("ConfigLoader: loading configuration.");
#endif
            if (AsyncMode)
            {
               var url = UnityEngine.Application.isEditor ? EditorConfigUrl : ConfigUrl;

#if DEBUG
               if (DebugMode)
                    Debug.Log(string.Format("ConfigLoader: loading in Async mode [{0}]", url));
#endif
                _connector = new HttpConnector
                                 {
                                     Url = url,
                                     CacheBuster = CacheBuster,
                                     FaultHandler = OnAsyncFault,
                                     ResponseMode = ResponseMode.WWW,
                                     Timeout = 30,
                                     //LogCalls = true
                                 };
                _connector.Send(new Responder(OnAsyncResult));
            }
            else
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log(string.Format("ConfigLoader: loading from Resources [{0}]", ConfigPath));
#endif

                _config = Resources.Load(ConfigPath);

                if (null == _config)
                {
                    string msg = string.Format(ConfigurationException.LoadingError);
#if DEBUG
                    if (DebugMode)
                        Debug.Log(msg);
#endif
                    //Alert.Show(msg, "Configuration error");
                    if (null != ResultHandler)
                        ResultHandler(msg);
                }
                else
                {
                    Configuration.Instance.ProcessConfig(_config.ToString());
                    if (null != ResultHandler)
                        ResultHandler(Configuration.Instance.Application);
                }
            }
        }

        #region Async handlers

        /// <summary>
        /// Result handler for async loading
        /// </summary>
        /// <param name="data"></param>
        private static void OnAsyncResult(object data)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("ConfigLoader.OnAsyncResult");
#endif
            string config = ((WWW)data).text;
            Configuration.Instance.ProcessConfig(config);

            if (null != ResultHandler)
                ResultHandler(config);
        }

        /// <summary>
        /// Fault handler for async loading
        /// </summary>
        /// <param name="data"></param>
        private static void OnAsyncFault(object data)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("ConfigLoader.OnAsyncFault");
#endif
            string msg = string.Format("{0}: {1}", ConfigurationException.LoadingError, data);
            Debug.Log(msg);

            if (null != FaultHandler)
                FaultHandler(msg);
        }

        #endregion

    }
}