using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Common;
using SWI.SoftStock.Common.Dto;
using SWI.SoftStock.Common.Wcf;
using SWI.SoftStock.ServerApps.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWI.SoftStock.Client.Repositories
{
    public class WcfRepository : IRepository
    {
        #region IRepository Members

        public Guid GetMachineId()
        {
            throw new NotImplementedException();
        }

        public Guid GetCompanyId()
        {
            throw new NotImplementedException();
        }

        public IList<SoftwareDto> GetSoftwareInfos()
        {
            throw new NotImplementedException();
        }

        public Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos)
        {
            throw new NotImplementedException();
        }

        public Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> softwareInfos)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<ISoftwareService>();
            var request = new SoftwareRequest {Softwares = softwareInfos.ToArray(), MachineUniqueId = machineId};
            return client.Add(request);
        }

        public Response SetProcess(ProcessDto process)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IProcessService>();
            var request = new ProcessRequest {Process = process};
            return client.Add(request);
        }

        public DataResponse GetData(Guid machineId)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IMachineService>();
            return client.GetData(machineId);
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IOperationSystemService>();
            var request = new OperationSystemRequest {MachineUniqueId = machineId, OperationSystem = operationSystem};
            return client.AddOperationSystem(request);
        }

        public OperationSystemDto GetOperationSystem()
        {
            throw new NotImplementedException();
        }

        public void SetOperationSystemId(Guid operationSystemId)
        {
            throw new NotImplementedException();
        }

        public Guid GetOperationSystemId()
        {
            throw new NotImplementedException();
        }

        public OperationModeDto GetOperationMode()
        {
            throw new NotImplementedException();
        }

        public Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto operationMode)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IOperationSystemService>();
            var request = new OperationModeRequest
            {
                MachineUniqueId = machineId, OperationSystemUniqueId = operationSystemId, OperationMode = operationMode
            };
            return client.AddOperationMode(request);
        }

        public UserDto GetUser()
        {
            throw new NotImplementedException();
        }

        public Response SetUser(Guid machineId, UserDto user)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IUserService>();
            var request = new UserRequest {MachineUniqueId = machineId, User = user};
            return client.Add(request);
        }

        public Response SetActivity(Guid machineId)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IMachineService>();
            return client.SetActivity(machineId);
        }

        public Response SetMachineInfo(MachineDto machineInfo)
        {
            var serviceLocator = new ServiceLocator(new Credentials("TestUser", AuthenticationType.None));
            var client = serviceLocator.GetServiceProxy<IMachineService>();
            var request = new MachineRequest {Machine = machineInfo};
            return client.Add(request);
        }

        public Response SetMachineId(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public void SetCompanyId(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public MachineDto GetMachineInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}