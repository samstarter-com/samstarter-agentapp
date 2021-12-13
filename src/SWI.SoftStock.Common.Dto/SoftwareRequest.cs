using System;
using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class SoftwareRequest : Request
    {
        [JsonProperty]
        public SoftwareStatusDto[] Softwares { get; set; }

        [JsonProperty]
        public Guid MachineUniqueId { get; set; }
    }
}