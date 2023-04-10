using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public class SPList
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        [JsonPropertyName("Title")]
        public string? Title { get; set; }
        [JsonPropertyName("Hidden")]
        public bool IsHidden { get; set; }
        [JsonPropertyName("BaseTemplate")]
        public long BaseTemplate { get; set; }
    }
}
