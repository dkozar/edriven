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

using eDriven.Core.Signals;
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// Mouse utility
    /// </summary>
    public class MouseUtil
    {
        /// <summary>
        /// Differentiates the mouse button for an event type
        /// </summary>
        /// <param name="button"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="middle"></param>
        /// <returns></returns>
        public static string DifferentiateMouseButton(int button, string left, string right, string middle)
        {
            string eventType = null;
            switch (button)
            {
                case 0:
                    eventType = left;
                    break;
                case 1:
                    eventType = right;
                    break;
                case 2:
                    eventType = middle;
                    break;
                default:
                    Debug.Log("Unknown mouse button");
                    break;
            }

            return eventType;
        }

        /// <summary>
        /// Differentiates the mouse button for an event type
        /// </summary>
        /// <param name="button"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="middle"></param>
        /// <returns></returns>
        public static Signal DifferentiateMouseButton(int button, Signal left, Signal right, Signal middle)
        {
            Signal signal = null;
            switch (button)
            {
                case 0:
                    signal = left;
                    break;
                case 1:
                    signal = right;
                    break;
                case 2:
                    signal = middle;
                    break;
                default:
                    Debug.Log("Unknown mouse button");
                    break;
            }

            return signal;
        }
    }
}