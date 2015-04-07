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

namespace eDriven.Core
{
    /// <summary>
    /// The info class
    /// </summary>
    public sealed class Info
    {
        public const string AssemblyName = "eDriven.Core";
        public const string AssemblyVersion = "2.4.0";
        public const string Author = "Danko Kozar";
        public const string Copyright = "Copyright (c) Danko Kozar 2010-2014";
        public const string Web = "edriven.dankokozar.com";

        public override string ToString()
        {
            return string.Format(@"[{0} {1}]
[by {2}, (C) {3}, {4}]", AssemblyName, AssemblyVersion, Author, Copyright, Web);
        }
    }
}
