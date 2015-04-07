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
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// NOTE: To use this class, it is imperative to force objects on the server-side JSON serializer
    /// Drupal 7 Example (rest_server.views.inc):
    /// -----
    /// private function render_json($data, $jsonp = FALSE) {
    /// $json = str_replace('\\/', '/', json_encode($data, JSON_FORCE_OBJECT)); // note JSON_FORCE_OBJECT here
    ///     if ($jsonp && isset($_GET['callback'])) {
    ///         return sprintf('%s(%s);', $_GET['callback'], $json);
    ///     }
    ///     return $json;
    /// } 
    /// </remarks>
    public class Node
    {
        [JsonFx.Json.JsonName("nid")]
        public int Nid;// "nid": "224",

        [JsonFx.Json.JsonName("vid")]
        public int Vid;// "vid": "224",

        [JsonFx.Json.JsonName("type")]
        public string Type; // "type": "article",

        [JsonFx.Json.JsonName("language")]
        public string Language = Constants.Undefined; // "language": "und",

        [JsonFx.Json.JsonName("title")]
        public string Title; // "title": "Abigo Aliquip",

        [JsonFx.Json.JsonName("body")]
        //public Dictionary<string, Dictionary<string, BodyValueObject>> Body = new Dictionary<string, Dictionary<string, BodyValueObject>>();
        public ValueConstruct<BodyValueObject> Body = new ValueConstruct<BodyValueObject>();

        [JsonFx.Json.JsonName("uid")]
        public int Uid; // "uid": "15",

        [JsonFx.Json.JsonName("status")]
        public int Status = 1; // "status": "1",

        [JsonFx.Json.JsonName("created")]
        public int Created; // "created": "1329154901",

        [JsonFx.Json.JsonName("changed")]
        public int Changed; // "changed": "1329168930",

        [JsonFx.Json.JsonName("comment")]
        public string Comment; // "comment": "2",

        [JsonFx.Json.JsonName("comment_count")]
        public int CommentCount; // "comment_count": "2",

        [JsonFx.Json.JsonName("promote")]
        public int Promote = 1; // "promote": "0",

        [JsonFx.Json.JsonName("sticky")]
        public int Sticky; // = "0"; // "sticky": "0",

        [JsonFx.Json.JsonName("tnid")] 
        public int Tnid; // = "0"; // "tnid": "0",

        [JsonFx.Json.JsonName("translate")]
        public string Translate; // "translate": "0",

        [JsonFx.Json.JsonName("uri")]
        public string Uri; // "uri": "http://edrivenunity.com/backend/edriven/node/224

        //public Node()
        //{
        //    Body.Add(Constants.Undefined, new Dictionary<string, BodyValueObject>());
        //    Body[Constants.Undefined].Add("0", new BodyValueObject());
        //}

        public string GetDefaultBody()
        {
            if (null != Body)
                return Body.GetDefaultValue();

            return null; // string.Empty;
        }

        public override string ToString()
        {
            return string.Format("Nid: {0}, Vid: {1}, Type: {2}, Language: {3}, Title: {4}, Uid: {5}, Status: {6}, Created: {7}, Changed: {8}, Comment: {9}, Promote: {10}, Sticky: {11}, Tnid: {12}, Translate: {13}, Uri: {14}", Nid, Vid, Type, Language, Title, Uid, Status, Created, Changed, Comment, Promote, Sticky, Tnid, Translate, Uri);
        }
    }
}