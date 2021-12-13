using System;
using Newtonsoft.Json;

namespace SWI.SoftStock.Common.Dto
{
    [JsonObject]
    public class SoftwareDto : IEquatable<SoftwareDto>
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public PublisherDto Publisher { get; set; }


        [JsonProperty]
        public string InstallDate { get; set; }


        [JsonProperty]
        public string Version { get; set; }


        [JsonProperty]
        public string SystemComponent { get; set; }


        [JsonProperty]
        public string WindowsInstaller { get; set; }


        [JsonProperty]
        public string ReleaseType { get; set; }

        #region IEquatable<SoftwareDto> Members

        public bool Equals(SoftwareDto other)
        {
            if (other == null)
                return false;

            if ((Name != other.Name)
                || (Version != other.Version)
                || (InstallDate != other.InstallDate)
                || (ReleaseType != other.ReleaseType)
                || (SystemComponent != other.SystemComponent)
                || (WindowsInstaller != other.WindowsInstaller)
                || ((Publisher != null) && (!Publisher.Equals(other.Publisher)))
                || ((Publisher == null) && (other.Publisher != null))
                )
                return false;

            return true;
        }

        #endregion

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            var currentObj = obj as SoftwareDto;
            return currentObj != null && Equals(currentObj);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                if (Name != null)
                {
                    hash = hash*23 + Name.GetHashCode();
                }
                if (Version != null)
                {
                    hash = hash*23 + Version.GetHashCode();
                }
                if (InstallDate != null)
                {
                    hash = hash*23 + InstallDate.GetHashCode();
                }
                if (ReleaseType != null)
                {
                    hash = hash*23 + ReleaseType.GetHashCode();
                }
                if (SystemComponent != null)
                {
                    hash = hash*23 + SystemComponent.GetHashCode();
                }
                if (WindowsInstaller != null)
                {
                    hash = hash*23 + WindowsInstaller.GetHashCode();
                }
                if (Publisher != null)
                {
                    hash = hash*23 + Publisher.GetHashCode();
                }
                return hash;
            }
        }

        public override string ToString()
        {
            return String.Format("Name:{0} Version:{1} InstallDate:{2} Publisher:{3}", Name, Version, InstallDate,
                                 Publisher);
        }
    }
}