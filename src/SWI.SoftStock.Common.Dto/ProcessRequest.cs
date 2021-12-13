using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class ProcessRequest : Request
    {
        [JsonProperty]
        public ProcessDto Process { get; set; }
    }
}