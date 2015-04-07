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

using UnityEngine;

namespace eDriven._3D.Events
{
    /// <summary>
    /// Dispatched by the CameraManager when camera changed
    /// </summary>
    public class CameraChangeEvent : Core.Events.Event
    {
// ReSharper disable InconsistentNaming
        public const string CAMERA_CHANGE = "cameraChange";
// ReSharper restore InconsistentNaming

        /// <summary>
        /// Old value
        /// </summary>
        public Camera PreviousCamera;

        /// <summary>
        /// New value
        /// </summary>
        public Camera NextCamera;

        public CameraChangeEvent(string type) : base(type)
        {
        }

        public CameraChangeEvent(string type, object target) : base(type, target)
        {
        }

        public CameraChangeEvent(string type, bool bubbles) : base(type, bubbles)
        {
        }

        public CameraChangeEvent(string type, bool bubbles, bool cancelable) : base(type, bubbles, cancelable)
        {
        }
    }
}