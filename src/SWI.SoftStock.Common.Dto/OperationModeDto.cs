using Newtonsoft.Json;
using System;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class OperationModeDto : IEquatable<OperationModeDto>
    {
        [JsonProperty]
        public string BootMode { get; set; }

        [JsonProperty]
        public string EnvironmentVariables { get; set; }

        [JsonProperty]
        public string LogicalDrives { get; set; }

        [JsonProperty]
        public bool Secure { get; set; }

        [JsonProperty]
        public string SerialNumber { get; set; }

        [JsonProperty]
        public string SystemDirectory { get; set; }

        #region IEquatable<OperationModeDto> Members

        public bool Equals(OperationModeDto other)
        {
            if (other == null)
                return false;

            if ((BootMode != other.BootMode)
                || (Secure != other.Secure)
                || (SystemDirectory != other.SystemDirectory)
                || (EnvironmentVariables != other.EnvironmentVariables)
                || (LogicalDrives != other.LogicalDrives)
                || (SerialNumber != other.SerialNumber)
                )
                return false;
            return true;
        }

        #endregion
    }
}