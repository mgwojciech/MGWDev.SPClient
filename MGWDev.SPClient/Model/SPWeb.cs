using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public class SPWeb
    {
        [JsonPropertyName("Created")]
        public DateTime? Created { get; set; }
        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        [JsonPropertyName("Title")]
        public string? Title { get; set; }
        [JsonPropertyName("WelcomePage")]
        public string? WelcomePage { get; set; }
        [JsonPropertyName("Language")]
        public uint Language { get; set; }
    }
}
