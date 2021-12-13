using System.ServiceModel;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface IOperationSystemService
    {
        [OperationContract]
        Response AddOperationSystem(OperationSystemRequest request);

        [OperationContract]
        Response AddOperationMode(OperationModeRequest request);
    }
}