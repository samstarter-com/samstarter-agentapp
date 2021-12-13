using System;
using System.ServiceModel;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface IMachineService
    {
        [OperationContract]
        Response Add(MachineRequest value);

        [OperationContract]
        DataResponse GetData(Guid value);

        [OperationContract]
        Response SetActivity(Guid value);
    }
}