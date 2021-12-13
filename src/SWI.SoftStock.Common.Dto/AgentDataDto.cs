using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class AgentDataDto
    {

        /// <summary>
        /// Agent's server call interval in ms
        /// </summary>

        [JsonProperty]
        public int Interval { get; set; }
    }
}