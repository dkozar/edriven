#region License

/*
 
Copyright (c) 2012 Danko Kozar

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

namespace eDriven.Core.Geom
{
    /// <summary>
    /// The class used for specifying edges (padding or margins)
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class EdgeMetrics : ICloneable
    {
        /// <summary>
        /// Zero metrics
        /// </summary>
        public static EdgeMetrics Zero
        {
            get
            {
                return new EdgeMetrics(0, 0, 0, 0);
            }
        }

        #region Properties

        /// <summary>
        /// The value representing the left edge
        /// </summary>
        public float Left;

        /// <summary>
        /// The value representing the right edge
        /// </summary>
        public float Right;

        /// <summary>
        /// The value representing the top edge
        /// </summary>
        public float Top;

        /// <summary>
        /// The value representing the bottom edge
        /// </summary>
        public float Bottom;

        /// <summary>
        /// The setter for all values
        /// </summary>
        public float All
        {
            set
            {
                Left = Right = Top = Bottom = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeMetrics()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeMetrics(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return string.Format("Left: {0}, Right: {1}, Top: {2}, Bottom: {3}", Left, Right, Top, Bottom);
        }

        #endregion

        #region Equals

        /// <summary>
        /// Equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(EdgeMetrics other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Left == Left && other.Right == Right && other.Top == Top && other.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(EdgeMetrics)) return false;
            return Equals((EdgeMetrics)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Left.GetHashCode();
                result = (result * 397) ^ Right.GetHashCode();
                result = (result * 397) ^ Top.GetHashCode();
                result = (result * 397) ^ Bottom.GetHashCode();
                return result;
            }
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            EdgeMetrics em = new EdgeMetrics();
            em.Left = Left;
            em.Right = Right;
            em.Top = Top;
            em.Bottom = Bottom;

            return em;
        }

        #endregion

    }
}