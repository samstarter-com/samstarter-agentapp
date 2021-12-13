using Newtonsoft.Json;
using System;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class ProcessDto
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public DateTime DateTime { get; set; }

        [JsonProperty]
        public ProcessStatus Status { get; set; }

        [JsonProperty]
        public Guid MachineObservableUniqueId { get; set; }
    }
}