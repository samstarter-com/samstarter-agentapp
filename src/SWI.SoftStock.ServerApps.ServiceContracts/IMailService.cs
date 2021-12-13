using System;
using System.ServiceModel;
using SWI.SoftStock.Common.Attributes;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.ServerApps.ServiceContracts
{
    [ServiceContract]
    public interface IMailService
    {
        [OperationContract]
        [Internationalization]
        Response SendMail(string email, string confirmationPath, Guid companyId);
    }
}