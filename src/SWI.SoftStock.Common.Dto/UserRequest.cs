using System;
using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public  class  UserRequest:Request
    {
        [JsonProperty]
        public UserDto User { get; set; }

        [JsonProperty]
        public Guid MachineUniqueId { get; set; }
    }
}