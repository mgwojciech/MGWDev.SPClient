﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Model
{
    public class CollectionResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
        [JsonPropertyName("odata.nextLink")]
        public string NextLink { get; set; }
    }
}
