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
using eDriven.Core.Serialization;
using UnityEngine;

namespace eDriven.Networking.Configuration
{
    /// <summary>
    /// Configuration is the wrapper around XmlSerializer
    /// It deserializes the XML configuration into the Application object
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;

        #region Singleton

        private static Configuration _instance;

        private Configuration()
        {
            // Constructor is protected!
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }

                return _instance;
            }
        }

        private Application _application;
        /// <summary>
        /// The result of deserialization is the Application object
        /// </summary>
        public Application Application
        {
            get
            {
                if (!Deserilized)
                    throw new ConfigurationException(ConfigurationException.NotDeserialized);

                if (null == _application)
                    throw new ConfigurationException(ConfigurationException.ConfigIsNull);

                return _application;
            }
            internal set
            {
                _application = value;
            }
        }

        #endregion

        /// <summary>
        /// The flag indicating if the configuration has already been processed
        /// </summary>
        public bool Deserilized { get; internal set; }

        /// <summary>
        /// Internal operation called by config loader
        /// </summary>
        /// <param name="xml"></param>
        internal void ProcessConfig(string xml)
        {
            //Initialized = false;
            Deserilized = false;

#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format(@"Processing configuration:
============================================
{0}
============================================", xml));
#endif

            //DispatchEvent(new Event(LOADED));

            try
            {
                _application = XmlSerializer<Application>.Deserialize(xml);
                
#if DEBUG
                string msg = string.Format(@"Configuration deserialized");
                if (DebugMode)
                    Debug.Log(msg);
#endif
                Deserilized = true;
            }
            catch (InvalidOperationException ex)
            {
                string msg = string.Format(@"{0}: 
{1}", ConfigurationException.DeserializationError, ex);

                Debug.Log(msg);
            }
            catch (Exception ex)
            {
                string msg = string.Format(@"{0}: 
{1}", ConfigurationException.DeserializationError, ex);

                Debug.Log(msg);
            }
        }
    }

    /// <summary>
    /// The exception that can be thrown by UiComponent
    /// </summary>
    public class ConfigurationException : Exception
    {
        public static string LoadingError = "Error loading configuration";
        public static string NotDeserialized = "Configuration not yet deserialized";
        public static string ConfigIsNull = "Application is null";
        public static string DeserializationError = "Error deserializing configuration";

        public ConfigurationException()
        {

        }

        /// <summary>
        /// Constructor
        ///</summary>
        ///<param name="message"></param>
        public ConfigurationException(string message)
            : base(message)
        {

        }
    }
}