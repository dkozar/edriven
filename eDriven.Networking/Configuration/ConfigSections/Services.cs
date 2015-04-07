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
using System.ComponentModel;
using System.Xml.Serialization;
using eDriven.Core.Serialization;
using eDriven.Networking.Rpc;

namespace eDriven.Networking.Configuration.ConfigSections
{
    /// <summary>
    /// Services
    /// </summary>
    [Serializable]
    [XmlRoot("Services", Namespace = "http://configuration.edriven.dankokozar.com/")]
    public class Services
    {
        ///<summary>
        /// HTTP connectors
        ///</summary>
        [XmlArray("Http")]
        [XmlArrayItem("HttpConnector")]
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public StringIndexedList<HttpConnector> Http = new StringIndexedList<HttpConnector>();
// ReSharper restore FieldCanBeMadeReadOnly.Global

        /// <summary>
        /// Default HTTP service name
        /// </summary>
        [XmlAttribute("DefaultHttpConnector")]
        [DefaultValue(null)]
        public string DefaultHttpServiceName;

        /// <summary>
        /// The default HTTP connector
        /// </summary>
        public HttpConnector DefaultHttpConnector
        {
            get
            {
                HttpConnector connector = Http.Find(delegate(HttpConnector s)
                                                    {
                                                        return s.Id == DefaultHttpServiceName;
                                                    });

                if (null == connector)
                    throw new Exception(string.Format("Default HttpConnector [Id={0}] not found", DefaultHttpServiceName));

                return connector;
            }
        }

// ReSharper disable EmptyConstructor
        /// <summary>
        /// Services
        /// </summary>
        public Services()
        {
        }
// ReSharper restore EmptyConstructor

        #region Handling Unknowns

        private System.Xml.XmlNode[] _unknownNodes;
        private System.Xml.XmlAttribute[] _unknownAttributes;

        [XmlAnyElement]
        public System.Xml.XmlNode[] UnknownNodes
        {
            get { return _unknownNodes; }
            set { _unknownNodes = value; }
        }

        [XmlAnyAttribute]
        public System.Xml.XmlAttribute[] UnknownAttributes
        {
            get { return _unknownAttributes; }
            set { _unknownAttributes = value; }
        }

        #endregion
    }
}