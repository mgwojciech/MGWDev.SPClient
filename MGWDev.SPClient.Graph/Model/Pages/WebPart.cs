using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Graph.Model.Pages
{
    public class WebPart<T>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("innerHtml")]
        public string InnerHtml { get; set; }
        [JsonPropertyName("webPartType")]
        public string WebPartType { get; set; }
        [JsonPropertyName("data")]
        public WebPartData<T> Data { get; set; }
    }
    public class WebPartData<T>
    {
        [JsonPropertyName("audiences")]
        public List<object> Audiences { get; set; }
        [JsonPropertyName("dataVersion")]
        public string DataVersion { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("properties")]
        public T Properties { get; set; }
        [JsonPropertyName("serverProcessedContent")]
        public ServerProcessedContent ServerProcessedContent { get; set; }
    }
    public class ServerProcessedContent
    {

    }
}
