using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class SoftwareStatusDto
    {
        [JsonProperty]
        public SoftwareDto Software { get; set; }

        [JsonProperty]
        public SoftwareStatus Status { get; set; }
    }


}