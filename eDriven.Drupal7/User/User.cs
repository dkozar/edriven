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

namespace eDriven.Drupal7.User
{
    public class User
    {
        [JsonFx.Json.JsonName("uid")]
        public string Uid;

        [JsonFx.Json.JsonName("name")]
        public string Name;

        [JsonFx.Json.JsonName("mail")]
        public string Mail;

        [JsonFx.Json.JsonName("theme")]
        public string Theme;

        [JsonFx.Json.JsonName("signature")]
        public string Signature;

        [JsonFx.Json.JsonName("signature_format")]
        public string SignatureFormat;

        [JsonFx.Json.JsonName("created")]
        public int Created;

        [JsonFx.Json.JsonName("access")]
        public int Access;

        [JsonFx.Json.JsonName("login")]
        public int Login;

        [JsonFx.Json.JsonName("status")]
        public int Status;

        [JsonFx.Json.JsonName("timezone")]
        public string Timezone;

        [JsonFx.Json.JsonName("language")]
        public string Language;

        [JsonFx.Json.JsonName("picture")]
        public string Picture;

        [JsonFx.Json.JsonName("init")]
        public string Init;

        //[JsonFx.Json.JsonName("data")]
        //public string Data; // NOTE: Error executing result responder: JsonFx.Json.JsonTypeCoercionException: Only objects with default constructors can be deserialized. (System.String)

        [JsonFx.Json.JsonName("roles")]
        public Dictionary<string, string> Roles;

        [JsonFx.Json.JsonName("type")]
        public string Type;
    }
}