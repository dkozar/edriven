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

namespace eDriven.Drupal7
{
    public class Comment
    {
        [JsonFx.Json.JsonName("cid")]
        public int Cid;

        [JsonFx.Json.JsonName("pid")]
        public int Pid;

        [JsonFx.Json.JsonName("nid")]
        public int Nid;

        [JsonFx.Json.JsonName("uid")]
        public int Uid;

        [JsonFx.Json.JsonName("subject")]
        public string Subject; // "type": "article",

        [JsonFx.Json.JsonName("created")] 
        public string Created;

        [JsonFx.Json.JsonName("name")]
        public string Name;

        [JsonFx.Json.JsonName("mail")]
        public string Mail;

        [JsonFx.Json.JsonName("homepage")]
        public string Homepage;

        [JsonFx.Json.JsonName("u_uid")]
        public int UUid;

        [JsonFx.Json.JsonName("comment_body")]
        public ValueConstruct<ValueObject> Body = new ValueConstruct<ValueObject>();
 
        public string GetDefaultBody()
        {
            if (null != Body)
                return Body.GetDefaultValue();

            return null; // string.Empty;
        }

        public override string ToString()
        {
            return string.Format("Cid: {0}, Pid: {1}, Nid: {2}, Uid: {3}, Subject: {4}, Created: {5}, Name: {6}, Mail: {7}, Homepage: {8}, UUid: {9}, Body: {10}", Cid, Pid, Nid, Uid, Subject, Created, Name, Mail, Homepage, UUid, Body);
        }
    }
}