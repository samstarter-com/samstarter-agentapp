using Newtonsoft.Json;
using System;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class UserDto : IEquatable<UserDto>
    {
        [JsonProperty]
        public string UserDomainName { get; set; }

        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public bool IsEmpty { get; set; }

        #region IEquatable<UserDto> Members

        public bool Equals(UserDto other)
        {
            if (other == null)
                return false;

            return IsEmpty == other.IsEmpty && UserDomainName == other.UserDomainName && UserName == other.UserName;
        }

        #endregion
    }
}