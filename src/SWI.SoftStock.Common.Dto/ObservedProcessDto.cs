using Newtonsoft.Json;
using System;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class ObservedProcessDto
    {
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public string ProcessName { get; set; }
    }
}