using System.Runtime.Serialization;

namespace SWI.SoftStock.Common.Dto
{
    [DataContract]
    public class MachineRequest : Request
    {
        [DataMember(Order = 0)]
        public MachineDto Machine { get; set; }
    }
}