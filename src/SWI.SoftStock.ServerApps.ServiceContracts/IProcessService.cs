using System.ServiceModel;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface IProcessService
    {
        [OperationContract]
        Response Add(ProcessRequest process);
    }
}