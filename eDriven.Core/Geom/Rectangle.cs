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
using UnityEngine;

namespace eDriven.Core.Geom
{
    /// <summary>
    /// The class used for specifying rectangular size and doing the rectangle math
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class Rectangle : ICloneable
    {
        #region Properties

        #region Values

        private float _x;
        /// <summary>
        /// X coordinate
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        private float _y;
        /// <summary>
        /// Y coordinate
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        private float _width;
        /// <summary>
        /// Width
        /// </summary>
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private float _height;
        /// <summary>
        /// Height
        /// </summary>
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Alias for X
        /// </summary>
        public float Left
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// X max getter
        /// </summary>
        public float Right
        {
            get
            {
                return _x + _width;
            }
            set
            {
                _width = value - _x;
            }
        }

        /// <summary>
        /// Alias for Y
        /// </summary>
        public float Top
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Y max getter
        /// </summary>
        public float Bottom
        {
            get
            {
                return _y + _height;
            }
            set
            {
                _height = value - _y;
            }
        }

        #endregion

        /// <summary>
        /// Returns rectangle position
        /// </summary>
        public Point Position
        {
            get
            {
                return new Point(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        public Point TopLeft
        {
            get
            {
                return new Point(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        public Point BottomRight
        {
            get
            {
                return new Point(Right, Bottom);
            }
            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        /// <summary>
        /// Returns rectangle size
        /// </summary>
        public Point Size
        {
            get
            {
                return new Point(_width, _height);
            }
            set
            {
                _width = value.X;
                _height = value.Y;
            }
        }

        /// <summary>
        /// Returns rectangle center point (used for rotation)
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(_x + _width/2, _y + _width/2);
            }
        }

        private Vector2 _center;
        public Vector2 CenterAsVector2
        {
            get
            {
                _center.x = _x + _width/2;
                _center.y = _y + _height/2;
                return _center;   
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Rectangle()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Rectangle(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Rectangle(Rect rect)
        {
            _x = rect.x;
            _y = rect.y;
            _width = rect.width;
            _height = rect.height;
        }

        #endregion

        #region Public Methods

        #region Creation

        /// <summary>
        /// Creates a rectangle from 2 points
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        /// <returns></returns>
        public static Rectangle From2Points(Point pStart, Point pEnd)
        {
            return new Rectangle(pStart.X, pEnd.Y, pEnd.X - pStart.X, pEnd.Y - pStart.Y);
        }

        /// <summary>
        /// Creates a rectangle from position and size
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle FromPositionAndSize(Point position, Point size)
        {
            return new Rectangle(position.X, position.Y, size.X, size.Y);
        }

        /// <summary>
        /// Creates a rectangle from size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Rectangle FromSize(Point size)
        {
            return new Rectangle(0, 0, size.X, size.Y);
        }

        /// <summary>
        /// Creates a rectangle from size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rectangle FromWidthAndHeight(float width, float height)
        {
            return new Rectangle(0, 0, width, height);
        }

        #endregion

        #region Moving

        /// <summary>
        /// Moves original
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle MoveTo(Point p)
        {
            _x = p.X;
            _y = p.Y;

            return this;
        }

        /// <summary>
        /// Moves a copy
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle MoveToCopy(Point p)
        {
            return new Rectangle(p.X, p.Y, Width, Height);
        }

        /// <summary>
        /// Moves original by delta
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle MoveBy(Point p)
        {
            _x += p.X;
            _y += p.Y;

            return this;
        }

        /// <summary>
        /// Moves copy by delta
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Rectangle MoveByCopy(Point p)
        {
            return new Rectangle(X + p.X, Y + p.Y, Width, Height);
        }

        #endregion
        
        #region Expand / Collapse

        /// <summary>
        /// Expands a rectangle
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public Rectangle Expand(float left, float right, float top, float bottom)
        {
            Rectangle r = (Rectangle)Clone();
            r.X -= left;
            r.Width += (left + right);
            r.Y -= top;
            r.Height += (top + bottom);
            return r;
        }

        /// <summary>
        /// Expands a rectangle
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Rectangle Expand(float amount)
        {
            return Expand(amount, amount, amount, amount);
        }

        /// <summary>
        /// Collapses a rectangle
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public Rectangle Collapse(float left, float right, float top, float bottom)
        {
            Rectangle r = (Rectangle) Clone();
            r.X += left;
            r.Width -= (left + right);
            r.Width = Math.Max(r.Width, 0);
            //r.Right -= right;
            r.Y += top;
            r.Height -= (top + bottom);
            r.Height = Math.Max(r.Height, 0);
            //r.Bottom -= bottom;
            return r;
        }

        /// <summary>
        /// Collapses a rectangle
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Rectangle Collapse(float amount)
        {
            return Collapse(amount, amount, amount, amount);
        }

        #endregion

        #region Centering

        /// <summary>
        /// Centers the inner rectangle inside the given outer rectangle
        /// </summary>
        /// <param name="outer">Outer rectangle</param>
        /// <param name="inner">Inner rectangle</param>
        /// <returns>Centered rectangle</returns>
        public static Rectangle CenterRectangle(Rectangle outer, Rectangle inner)
        {
            return new Rectangle((outer.Width - inner.Width) / 2, (outer.Height - inner.Height) / 2, inner.Width, inner.Height);
        }

        /// <summary>
        /// Centers THIS rectangle inside the given outer rectangle
        /// </summary>
        /// <param name="outerRectangle"></param>
        /// <returns>Centered (THIS) rectangle</returns>
        public Rectangle CenterInside(Rectangle outerRectangle)
        {
            //Debug.Log("outerRectangle: " + outerRectangle);
            return CenterRectangle(outerRectangle, this);
        }

        /// <summary>
        /// Gets align bounds
        /// </summary>
        /// <param name="childrenBounds"></param>
        /// <param name="containerBounds"></param>
        /// <returns></returns>
        public static Rectangle GetAlignBounds(Rectangle childrenBounds, Rectangle containerBounds)
        {
            float width = Math.Max(childrenBounds.Width, containerBounds.Width);
            float height = Math.Max(childrenBounds.Height, containerBounds.Height);
            return new Rectangle(0, 0, width, height);
        }

        #endregion

        #region Point Operations

        /// <summary>
        /// Checks if a point is placed inside rectangle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point)
        {
            // inversion, early return
            if (point.X < X || point.Y <= Y || point.X >= Right || point.Y > Bottom)
                return false;

            return true;
        }

        #endregion

        #region Rect operations

        /// <summary>
        /// Returns true if two rectangles intersect
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle rectangle)
        {
            // reverse logic - early exit
            return !(Left > rectangle.Right ||
               Right < rectangle.Left ||
               Top > rectangle.Bottom ||
               Bottom < rectangle.Top);
        }

        /// <summary>
        /// Checks if a given rectangle is placed inside this rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool Contains(Rectangle rectangle)
        {
            return (rectangle.Left >= Left &&
               rectangle.Right <= Right &&
               rectangle.Top >= Top &&
               rectangle.Bottom <= Bottom);
        }

        /// <summary>
        /// Constrains this rectangle inside another rectangle's bounds
        /// </summary>
        /// <param name="rectangle"></param>
        public void ConstrainWithin(Rectangle rectangle)
        {
            X = Math.Max(X, rectangle.X);
            Y = Math.Max(Y, rectangle.Y);
            Right = Math.Min(Right, rectangle.Right);
            Bottom = Math.Min(Bottom, rectangle.Bottom);
        }

        #endregion

        #region Rect conversions

        /// <summary>
        /// Converts to UnityEngine.Rect
        /// </summary>
        /// <returns></returns>
        public Rect ToRect()
        {
            return new Rect(_x, _y, _width, _height);
        }

        /// <summary>
        /// Converts from UnityEngine.Rect
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rectangle FromRect(Rect r)
        {
            return new Rectangle(r.x, r.y, r.width, r.height);
        }

        #endregion

        #endregion

        #region Static

        /// <summary>
        /// Zero rectangle
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public static Rectangle Zero
        // ReSharper restore UnusedMember.Global
        {
            get
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return string.Format("Rectangle[X:{0}, Y:{1}, Width:{2}, Height:{3}]", _x, _y, _width, _height);
        }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true; // commented 20120625
            if (obj.GetType() != typeof(Rectangle)) return false;
            return Equals((Rectangle)obj);
        }

        /// <summary>
        /// Equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Rectangle other)
        {
            if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true; // commented 20120625
            return other._x == _x && other._y == _y && other._width == _width && other._height == _height;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _x.GetHashCode();
                result = (result * 397) ^ _y.GetHashCode();
                result = (result * 397) ^ _width.GetHashCode();
                result = (result * 397) ^ _height.GetHashCode();
                return result;
            }
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        #endregion
    }
}