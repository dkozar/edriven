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

namespace eDriven.Core.Tasks
{
    /// <summary>
    /// Runs an async process
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// The description for the job
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Runs the worker
        /// </summary>
        void Run();

        /// <summary>
        /// The indicator if the worker is done
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Token (used for checking progress etc.)
        /// </summary>
        Token Token { get; }

        /// <summary>
        /// Heartbeat
        /// </summary>
        void Tick();

        /// <summary>
        /// Starting time
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// A callback that should fire after the job is done
        /// </summary>
        TaskQueue.Callback Callback { get; set; }

        /// <summary>
        /// A flag indicating that this job is included in progress
        /// </summary>
        bool Excluded { get; }
    }
}