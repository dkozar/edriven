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

namespace eDriven.Core.Geom
{
    /// <summary>
    /// The class used for specifying constrains
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class ConstraintMetrics // : IEquatable<ConstraintMetrics>
    {
        /// <summary>
        /// Zero metrics
        /// </summary>
        public static ConstraintMetrics Zero
        {
            get
            {
                return new ConstraintMetrics(0, 0, 0, 0);
            }
        }

        #region Properties

        /// <summary>
        /// Left offset
        /// </summary>
        public float? Left;

        /// <summary>
        /// Right offset
        /// </summary>
        public float? Right;

        /// <summary>
        /// Top offset
        /// </summary>
        public float? Top;

        /// <summary>
        /// Bottom offset
        /// </summary>
        public float? Bottom;

        /// <summary>
        /// Width
        /// </summary>
        public float? Width;

        /// <summary>
        /// Height
        /// </summary>
        public float? Height;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstraintMetrics()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstraintMetrics(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstraintMetrics(float? left, float? right, float? top, float? bottom, float? width, float? height)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Width = width;
            Height = height;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return string.Format("Left: {0}, Right: {1}, Top: {2}, Bottom: {3}, Width: {4}, Height: {5}", Left, Right, Top, Bottom,Width, Height);
        }

        #endregion

        #region Equals

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(ConstraintMetrics other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.Width == Width && other.Height == Height;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        ///                 </exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ConstraintMetrics);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ Width.GetHashCode();
                result = (result*397) ^ Height.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ConstraintMetrics left, ConstraintMetrics right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ConstraintMetrics left, ConstraintMetrics right)
        {
            return !Equals(left, right);
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
            ConstraintMetrics metrics = new ConstraintMetrics
                                            {
                                                Left = Left,
                                                Right = Right,
                                                Top = Top,
                                                Bottom = Bottom,
                                                Width = Width,
                                                Height = Height
                                            };

            return metrics;
        }

        #endregion

        /// <summary>
        /// Constrains this rectangle with the outer one
        /// </summary>
        /// <param name="outer"></param>
        /// <returns></returns>
        public Rectangle GetConstrainedRectangle(Rectangle outer)
        {
            Rectangle rect = new Rectangle();

            if (null != Left)
                rect.X = outer.X + (float) Left;

            if (null != Width)
                rect.Width = (float)Width;
            else if (null != Right)
                rect.Width = outer.Width - rect.X - (float)Right;

            if (null != Top)
                rect.Y = outer.Y + (float)Top;

            if (null != Height)
                rect.Height = (float)Height;
            else if (null != Bottom)
                rect.Height = outer.Height - rect.Y - (float)Bottom;

            //Debug.Log(("   -> result: " + rect));

            return rect;
        }

    }
}