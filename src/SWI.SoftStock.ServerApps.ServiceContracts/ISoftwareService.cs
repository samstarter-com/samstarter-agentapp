using System.ServiceModel;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface ISoftwareService
    {
        [OperationContract]
        Response Add(SoftwareRequest request);
    }
}