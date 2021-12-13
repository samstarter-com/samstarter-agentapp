using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class DataResponse : Response
    {
        [JsonProperty]
        public ObservedProcessDto[] ObservedProcesses { get; set; }

        [JsonProperty]
        public AgentDataDto AgentData { get; set; }
    }
}