using System;
using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class OperationModeRequest : Request
    {
        [JsonProperty]
        public OperationModeDto OperationMode { get; set; }

        [JsonProperty]
        public Guid MachineUniqueId { get; set; }

        [JsonProperty]
        public Guid OperationSystemUniqueId { get; set; }
    }
}