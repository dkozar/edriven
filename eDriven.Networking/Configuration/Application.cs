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
using System.Xml.Serialization;
using eDriven.Core.Serialization;
using eDriven.Networking.Configuration.ConfigSections;

namespace eDriven.Networking.Configuration
{
    /// <summary>
    /// The Application object
    /// </summary>
    [Serializable]
    [XmlRoot("Configuration", Namespace = "http://edriven.dankokozar.com/")]
// ReSharper disable ClassNeverInstantiated.Global
    public class Application
// ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [XmlAttribute("Id")]
        public string Id;

        /// <summary>
        /// Services
        /// </summary>
        [XmlElement("Services")]
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public Services Services = new Services();
// ReSharper restore FieldCanBeMadeReadOnly.Global

        /// <summary>
        /// Properties
        /// </summary>
        [XmlArray("Properties")]
        [XmlArrayItem("Add")]
        public StringIndexedList<ConfigurationProperty> Properties = new StringIndexedList<ConfigurationProperty>();

        #region Handling Unknowns

        private System.Xml.XmlNode[] _unknownNodes;
        private System.Xml.XmlAttribute[] _unknownAttributes;

        /// <summary>
        /// Deserialization stuff
        /// </summary>
        [XmlAnyElement]
        public System.Xml.XmlNode[] UnknownNodes
        {
            get { return _unknownNodes; }
            set { _unknownNodes = value; }
        }

        /// <summary>
        /// Deserialization stuff
        /// </summary>
        [XmlAnyAttribute]
        public System.Xml.XmlAttribute[] UnknownAttributes
        {
            get { return _unknownAttributes; }
            set { _unknownAttributes = value; }
        }

        #endregion
    }
}