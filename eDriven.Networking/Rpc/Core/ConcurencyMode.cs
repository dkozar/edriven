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

namespace eDriven.Networking.Rpc
{
    /// <summary>
    /// Concurency mode describes the order and timing of requests
    /// N = MaxConcurrentRequests
    /// </summary>
    public enum ConcurencyMode
    {
        // Multiple requests alowed
        Multiple = 1, 
        
        // Only N requests at a time is allowed
        // Additional requests are being canceled
        SingleFirst = 2,

        // Only N requests at a time is allowed
        // Previous requests are being canceled
        SingleLast = 3,

        // Making N requests at a time but queueing additional requests
        // Processing in FIFO order (first in - first out)
        FifoQueued = 4,

        // Making N requests at a time but queueing additional requests
        // Processing in FILO order (first in - last out)
        FiloQueued = 5
    }
}