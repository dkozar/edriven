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

using eDriven.Networking.External;
using UnityEngine;

namespace eDriven.Networking.Util
{
    /// <summary>
    /// The logging class
    /// Loggs to editor or browser console
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Should I skip logging when in web player and not registered with Javascript?
        /// </summary>
        public static bool WaitForRegistration;

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            if (Application.isWebPlayer)
            {
                if (!WaitForRegistration) {
                    JavascriptConnector.Instance.Call("log", null, message);
                    return;
                }

                if (JavascriptConnector.Registered)
                    JavascriptConnector.Instance.Call("log", null, message);
                else
                    Debug.Log(message);
            }
            else if (Application.isEditor)
            {
                Debug.Log(message);
            }
            else
            {
                Debug.Log(message);
            }
        }
    }
}