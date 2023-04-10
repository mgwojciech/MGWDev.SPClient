using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public class SPUser
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string ClaimName { get; set; }
        [JsonPropertyName("Title")]
        public string FullName { get; set; }
        [JsonPropertyName("EMail")]
        public string Email { get; set; }
    }
}
