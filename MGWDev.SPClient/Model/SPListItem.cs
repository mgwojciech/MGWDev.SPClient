using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public class SPListItem
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Title")]
        public string? Title { get; set; }
        [JsonPropertyName("Author")]
        public SPUser Author { get; set; }
    }
}
