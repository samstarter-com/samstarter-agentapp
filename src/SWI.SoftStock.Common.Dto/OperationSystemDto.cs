using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    [DataContract]
    public class OperationSystemDto : IEquatable<OperationSystemDto>
    {
        [JsonProperty]
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [JsonProperty]
        [DataMember(Order = 1)]
        public string Version { get; set; }

        [JsonProperty]
        [DataMember(Order = 2)]
        public uint MaxNumberOfProcesses { get; set; }

        [JsonProperty]
        [DataMember(Order = 3)]
        public ulong MaxProcessMemorySize { get; set; }

        [JsonProperty]
        [DataMember(Order = 4)]
        public string Architecture { get; set; }

        [JsonProperty]
        [DataMember(Order = 5)]
        public string BuildNumber { get; set; }

        [JsonProperty]
        [DataMember(Order = 6)]
        public Guid UniqueId { get; set; }

        #region IEquatable<OperationSystemDto> Members

        public bool Equals(OperationSystemDto other)
        {
            if (other == null)
                return false;

            if ((Name != other.Name)
                || (Version != other.Version)
                || (MaxNumberOfProcesses != other.MaxNumberOfProcesses)
                || (MaxProcessMemorySize != other.MaxProcessMemorySize)
                || (Architecture != other.Architecture)
                || (BuildNumber != other.BuildNumber)
                )
                return false;
            return true;
        }

        #endregion

        public override string ToString()
        {
            return
                String.Format(
                    "Name:{0} Version:{1} MaxNumberOfProcesses:{2} MaxProcessMemorySize:{3} Architecture:{4} BuildNumber:{5}",
                    Name, Version, MaxNumberOfProcesses, MaxProcessMemorySize, Architecture, BuildNumber);
        }
    }
}