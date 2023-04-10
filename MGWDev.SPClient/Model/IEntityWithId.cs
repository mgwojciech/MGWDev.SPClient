using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public interface IEntityWithId
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }
    }
}
