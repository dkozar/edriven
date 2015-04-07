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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace eDriven.Core.Util
{
    /// <summary>
    /// A helper for making a log strings
    /// </summary>
    public class LogUtil
    {
        /// <summary>
        /// Prepends a number of characters to the input string
        /// Use space character for tabbing
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Prepend(string input, int count, string c)
        {
            string output = string.Empty;

            for (int i = 0; i < count; i++)
            {
                output += c;
            }

            return output + input;
        }

        /// <summary>
        /// Gets current method name
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void PrintCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            Debug.Log(string.Format("{0}->{1}", sf.GetMethod().DeclaringType, sf.GetMethod().Name));
        }
    }
}
