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
using eDriven.Core.Geom;

namespace eDriven.Core.Events
{
    /// <summary>
    /// The event that has both global and local position
    /// This event bubbles by default
    /// </summary>
    /// <remarks>Coded by Danko Kozar</remarks>
    public class MouseEvent : InputEvent
    {
        #region Constants

// ReSharper disable InconsistentNaming
        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_MOVE = "mouseMove";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_OVER = "mouseOver";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_OUT = "mouseOut";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_DRAG = "mouseDrag";

        /// <summary>
        /// Constant
        /// </summary>
        public const string ROLL_OVER = "rollOver";

        /// <summary>
        /// Constant
        /// </summary>
        public const string ROLL_OUT = "rollOut";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_LEAVE = "mouseLeave";
        
        // left button

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_DOWN = "mouseDown";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_UP = "mouseUp";

        /// <summary>
        /// Constant
        /// </summary>
        public const string CLICK = "click";

        /// <summary>
        /// Constant
        /// </summary>
        public const string DOUBLE_CLICK = "doubleClick";

        // middle button

        /// <summary>
        /// Constant
        /// </summary>
        public const string MIDDLE_MOUSE_DOWN = "middleMouseDown";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MIDDLE_MOUSE_UP = "middleMouseUp";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MIDDLE_CLICK = "middleClick";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MIDDLE_DOUBLE_CLICK = "middleDoubleClick";

        // right button

        /// <summary>
        /// Constant
        /// </summary>
        public const string RIGHT_MOUSE_DOWN = "rightMouseDown";

        /// <summary>
        /// Constant
        /// </summary>
        public const string RIGHT_MOUSE_UP = "rightMouseUp";

        /// <summary>
        /// Constant
        /// </summary>
        public const string RIGHT_CLICK = "rightClick";

        /// <summary>
        /// Constant
        /// </summary>
        public const string RIGHT_DOUBLE_CLICK = "rightDoubleClick";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_WHEEL = "mouseWheel";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_DOWN_OUTSIDE = "mouseDownOutside";

        /// <summary>
        /// Constant
        /// </summary>
        public const string MOUSE_WHEEL_OUTSIDE = "mouseWheelOutside";
        
// ReSharper restore InconsistentNaming

        #endregion

        #region Properties

        /// <summary>
        /// The local position (used by GUI)
        /// </summary>
        public Point LocalPosition;

        /// <summary>
        /// The global (screen) position
        /// </summary>
        public Point GlobalPosition;

        /// <summary>
        /// Related object
        /// </summary>
        public object RelatedObject;

        /// <summary>
        /// True if the mouse button is down
        /// </summary>
        public bool ButtonDown;

        /// <summary>
        /// True if the mouse button is down
        /// </summary>
        public bool MiddleButtonDown;

        /// <summary>
        /// True if the mouse button is down
        /// </summary>
        public bool RightButtonDown;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseEvent(string type) : base(type)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseEvent(string type, object target) : base(type, target)
        {
            Bubbles = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseEvent(string type, bool bubbles, bool cancelable) : base(type, bubbles, cancelable)
        {
        }

        #endregion

        public override string ToString()
        {
            return string.Format("MouseEvent [{0}], GlobalPosition: {1}, LocalPosition: {2}", Type, GlobalPosition, LocalPosition);
        }
    }
}