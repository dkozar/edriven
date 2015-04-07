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
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

namespace eDriven.Networking.Rpc
{
    /// <summary>
    /// WebRequest class was born from the need of having something that doesn't execute the moment it is instantiated<br/>
    /// (like the WWW class does)<br/>
    /// It has all the confug characteristics of the WWW class, and the method that instantiates WWW at time needed
    /// </summary>
    public class WebRequest : IDisposable
    {
        public string Url;
        public int? Version;
        public Hashtable Headers;
        public byte[] PostData;
        public WWWForm Form;
        public bool CacheBuster;

        private WWW _www;

        #region Constructors

        public WebRequest(string url)
        {
            Url = url;
        }

        [Obsolete("WWW.LoadFromCacheOrDownload is unstable")]
        public WebRequest(string url, int version)
        {
            Url = url;
            Version = version;
        }

        public WebRequest(string url, byte[] postData)
        {
            Url = url;
            PostData = postData;
        }

        public WebRequest(string url, WWWForm form)
        {
            Url = url;
            Form = form;
        }

        public WebRequest(string url, byte[] postData, Hashtable headers)
        {
            Url = url;
            PostData = postData;
            Headers = headers;
        }

        public WebRequest(string url, WWWForm form, Hashtable headers)
        {
            Url = url;
            Form = form;
            Headers = headers;
        }

        public WebRequest(string url, Hashtable headers)
        {
            Url = url;
            Headers = headers;
        }

        #endregion

        /// <summary>
        /// Instantiates WWW
        /// </summary>
        /// <returns>WWW instance</returns>
        /// <remarks>WWW actualy calls the URL the moment is is created</remarks>
        public WWW CreateWww()
        {
            if (!string.IsNullOrEmpty(Url))
            {
                if (CacheBuster)
                    Url += string.Format("?{0}", (DateTime.Now - new DateTime(1970, 1, 1)).Ticks);

                if (null != Form)
                {
                    _www = new WWW(Url, Form);
                }
                else if (null != PostData)
                {
                    _www = null != Headers
                              ? new WWW(Url, PostData, Headers)
                              : new WWW(Url, PostData);
                }
                else if (null != Version) // load from cache
                {
                    _www = WWW.LoadFromCacheOrDownload(Url, (int)Version);
                }
                else
                {
                    _www = new WWW(Url);
                }
            }

            else
                throw new WebRequestException(WebRequestException.UrlNotDefined);

            return _www;
        }

        public override string ToString()
        {
            string postDataAsString = string.Empty;
            if (null != PostData)
            {
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                postDataAsString = enc.GetString(PostData);
            }

            string h = string.Empty;
            if (null != Headers && Headers.Count > 0)
            {
                foreach (var key in Headers.Keys)
                {
                    h += string.Format(@"    -> {0}: {1}
", key, Headers[key]);
                }
            }


            return
                string.Format(
                    @"WwwRequest
============
Url: {0}
Headers: 
{1}
PostData: {2}
Form: {3}
============", 
                    Url ?? "-",
                    string.Empty != h ? h : "-",
                    null == PostData ? "-" : postDataAsString, 
                    null != Form ? "Yes" : "No");
        }

        public void Dispose()
        {
            if (null != _www)
                _www.Dispose();
        }
    }

    public class WebRequestException : Exception
    {
        public static string UrlNotDefined = "WebRequest doesn't have URL defined";

        public WebRequestException()
        {
        }

        public WebRequestException(string message) : base(message)
        {
        }

        public WebRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WebRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}