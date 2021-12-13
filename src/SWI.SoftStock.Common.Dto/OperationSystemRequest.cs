using Newtonsoft.Json;
using System;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class OperationSystemRequest : Request
    {
        [JsonProperty]
        public OperationSystemDto OperationSystem { get; set; }

        [JsonProperty]
        public Guid MachineUniqueId { get; set; }
    }
}