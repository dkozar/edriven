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

namespace eDriven.Core.Events
{
    /// <summary>
    /// The event phase flags
    /// Used by systems that support event bubbling
    /// Describes the phase in the process of transfering an event of the particular child in the hierarchy
    /// The idea behind this enumerator is to have 6 combos, instead of only 2 (bubbling/capture phase)
    /// </summary>
    [Flags]
    public enum EventPhase
    {
        /// <summary>
        /// Capture phase
        /// A phase when an event is transfered from top-most parent to child and dispatched by each component
        /// </summary>
        Capture = 1, //0x0,

        /// <summary>
        /// Target phase
        /// The phase when the event is dispatched by target
        /// </summary>
        Target = 2, //0x1,

        /// <summary>
        /// Bubbling phase
        /// The phase when the event bubbles from child to top-most parent and dispatched by each component
        /// </summary>
        Bubbling = 4, //0x2

        /// <summary>
        /// Capture and target phase
        /// </summary>
        CaptureAndTarget = 3, //Capture | Target,

        /// <summary>
        /// Target and bubbling phase
        /// </summary>
        TargetAndBubbling = 6 //Target | Bubbling
    }
}