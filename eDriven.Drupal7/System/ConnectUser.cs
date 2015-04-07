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

using System.Collections.Generic;

namespace eDriven.Drupal7.System
{
    /// <summary>
    /// "user":{"uid":0,"hostname":"89.164.206.100","roles":{"1":"anonymous user"},"cache":0}}
    /// </summary>
    public class ConnectUser
    {
        [JsonFx.Json.JsonName("uid")]
        public int Uid;

        [JsonFx.Json.JsonName("hostname")]
        public string Hostname;

        [JsonFx.Json.JsonName("roles")]
        public Dictionary<string, string> Roles;

        [JsonFx.Json.JsonName("cache")] 
        public int Cache;
    }
}
