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

using System.Text;

namespace eDriven.Core.Signals
{
    public static class SignalUtil
    {
        private static StringBuilder _sb;
        private static string _s;

        public static string DescribeParams(params object[] parameters)
        {
            if (null == _sb)
                _sb = new StringBuilder();

            var count = parameters.Length;
            for (int i = 0; i < count; i++)
            {
                _sb.AppendLine(string.Format("    [{0}] {1} [{2}]", i, parameters[i], parameters[i].GetType()));
            }

            _s = _sb.ToString();
            _sb.Remove(0, _sb.Length);
            return _s;
        }
    }
}
