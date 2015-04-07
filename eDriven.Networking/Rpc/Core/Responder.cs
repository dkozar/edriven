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

namespace eDriven.Networking.Rpc
{
    /// <summary>
    /// Responder<br/>
    /// Also called "a promise"<br/>
    /// Asynchronuous services usually don't respond immediatelly, and are non-blocking<br/>
    /// Responder is the way to supply callback functions that should be called in the case of: 1) success; 2) fault
    /// </summary>
    public class Responder : IResponder
    {
        #region Callback definition

        private ResultHandler _resultHandler;
        public ResultHandler ResultHandler
        {
            get { return _resultHandler; }
            set { _resultHandler = value; }
        }

        private FaultHandler _faultHandler;
        public FaultHandler FaultHandler
        {
            get { return _faultHandler; }
            set { _faultHandler = value; }
        }

        #endregion

        #region Constructor

        public Responder()
        {
        }

        public Responder(ResultHandler resultHandler)
        {
            _resultHandler = resultHandler;
        }

        public Responder(ResultHandler resultHandler, FaultHandler faultHandler)
        {
            _resultHandler = resultHandler;
            _faultHandler = faultHandler;
        }

        #endregion

        #region Implementation of IResponder

        public void Result(object data)
        {
            if (null != _resultHandler)
                _resultHandler(data);
            else
                throw new Exception("ResultHandler not defined");
        }

        public void Fault(object info)
        {
            if (null != _faultHandler)
                _faultHandler(info);
        }

        #endregion
    }

    //public delegate void ResultHandler(WWW data);
    public delegate void ResultHandler(object result);

    public delegate void FaultHandler(object fault);
}
