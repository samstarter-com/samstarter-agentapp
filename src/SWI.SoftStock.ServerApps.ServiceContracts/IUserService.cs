using System.ServiceModel;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Response Add(UserRequest request);
    }
}