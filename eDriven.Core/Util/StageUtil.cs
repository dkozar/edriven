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

using eDriven.Core.Geom;
using UnityEngine;

namespace eDriven.Core.Util
{
    /// <summary>
    /// The utility class used for stage operations
    /// </summary>
    public class StageUtil
    {
        /// <summary>
        /// Offsets Rect
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="offsetLeft"></param>
        /// <param name="offsetTop"></param>
        /// <returns></returns>
        public static Rect OffsetRect(Rect rectangle, float offsetLeft, float offsetTop)
        {
            float left = rectangle.xMin + offsetLeft;
            float top = rectangle.yMin + offsetTop;
            return new Rect(left, top, rectangle.width, rectangle.height);
        }

        /// <summary>
        /// Centers Rect
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Rect CenterRect(Rect rectangle)
        {
            return new Rect((Screen.width - rectangle.width) / 2, (Screen.height - rectangle.height) / 2, rectangle.width, rectangle.height);
        }

        /// <summary>
        /// Centers the inner inside the outer Rect
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public static Rect CenterRect(Rect inner, Rect outer)
        {
            return new Rect((outer.width - inner.width) / 2, (outer.height - inner.height) / 2, inner.width, inner.height);
        }

        /// <summary>
        /// Centers the inner inside the outer Rectangle
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public static Rectangle CenterRect(Rectangle inner, Rectangle outer)
        {
            //Debug.Log("inner: " + inner + "; outer: " + outer);
            return new Rectangle((outer.Width - inner.Width) / 2, (outer.Height - inner.Height) / 2, inner.Width, inner.Height);
        }

        /// <summary>
        /// Flips the Y vector coordinate
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 FlipY(Vector3 v)
        {
            return new Vector3(v.x, Screen.height - v.y, v.z);
        }

// ReSharper disable InconsistentNaming
        /// <summary>
        /// Null Rect
        /// </summary>
        public static Rect NULL_RECT = new Rect(0, 0, 0, 0);
// ReSharper restore InconsistentNaming
    }
}