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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace eDriven.Core.Serialization
{
    /// <summary>
    /// XML serializer used for config etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class XmlSerializer<T>
    {
        /// <summary>
        /// Indent
        /// </summary>
        public static bool Indent = true;

        /// <summary>
        /// Indent chars
        /// </summary>
        public static string IndentChars = "\t";
        
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static T Deserialize(String pXmlizedString)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            MemoryStream memoryStream = new MemoryStream(StringToUtf8ByteArray(pXmlizedString));
            return (T)xs.Deserialize(memoryStream);
        }

        /// <summary>
        /// Deserializes XML string to object of type T
        /// </summary>
        /// <param name="toType"></param>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static T Deserialize(Type toType, String pXmlizedString)
        {
            XmlSerializer xs = new XmlSerializer(toType);

            MemoryStream memoryStream = new MemoryStream(StringToUtf8ByteArray(pXmlizedString));
            return (T)xs.Deserialize(memoryStream);
        }

        /// <summary>
        /// Deserializes XML string to object of type T
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static T Deserialize(Byte[] byteArray)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            return (T)xs.Deserialize(memoryStream);
        }

        /// <summary>
        /// Serializes object to XML string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize(object o)
        {
            XmlWriterSettings settings = new XmlWriterSettings {IndentChars = IndentChars, Indent = Indent};

            string xml;

            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter xw = XmlWriter.Create(stream, settings))
                {
// ReSharper disable AssignNullToNotNullAttribute
                    new XmlSerializer(typeof(T)).Serialize(xw, o);
// ReSharper restore AssignNullToNotNullAttribute
                }
                xml = Utf8ByteArrayToString(stream.ToArray());
            }

            return xml;
        }

        #region Helper

        private static Byte[] StringToUtf8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        private static String Utf8ByteArrayToString(Byte[] bytes)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string s = encoding.GetString(bytes);
            return s;
        }

        #endregion

    }
}