using MGWDev.SPClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Tests.Model
{
    public class InformationMessage : SPListItem
    {
        [JsonPropertyName("ValoMessageDescription")]
        public string? Body { get; set; }
        [JsonPropertyName("ValoMessageStartDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("ValoMessageEndDate")]
        public DateTime? EndDate { get; set; }
    }
}
