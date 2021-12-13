using System.ServiceModel;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface ICheckCompanyService
    {
        [OperationContract]
        int Check(string uniqueCompanyId);
    }
}
