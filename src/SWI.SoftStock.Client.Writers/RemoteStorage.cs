using System;
using System.Collections.Generic;
using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.Client.Storages
{
    public class RemoteStorage : Storage, IStorage
    {
        public RemoteStorage(IRepository repository) : base(repository)
        {
        }

        #region IStorage Members

        public Guid GetMachineId()
        {
            return Guid.Empty;
        }

        public Guid GetCompanyId()
        {
            return Guid.Empty;
        }

        public void SetCompanyId(Guid companyId)
        {
        }

        public Response SetMachineId(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public MachineDto GetMachineInfo()
        {
            throw new NotImplementedException();
        }

        public IList<SoftwareDto> GetSoftwareInfos()
        {
            return Repository.GetSoftwareInfos();
        }

        public Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos)
        {
            return Repository.SetSoftwareInfos(softwareInfos);
        }

        public Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> softwareInfos)
        {
            return Repository.SetModifiedSoftwareInfos(machineId, softwareInfos);
        }

        public DataResponse GetData(Guid machineId)
        {
            return Repository.GetData(machineId);
        }

        public OperationSystemDto GetOperationSystem()
        {
            throw new NotImplementedException();
        }

        public Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem)
        {
            return Repository.SetOperationSystem(machineId, operationSystem);
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
            return Repository.SetOperationMode(machineId, operationSystemId, operationMode);
        }

        public UserDto GetUser()
        {
            throw new NotImplementedException();
        }

        public Response SetUser(Guid machineId, UserDto user)
        {
            return Repository.SetUser(machineId, user);
        }

        public Response SetActivity(Guid machineId)
        {
            return Repository.SetActivity(machineId);
        }

        public Response SetProcess(ProcessDto process)
        {
            return Repository.SetProcess(process);
        }

        public Response SetMachineInfo(MachineDto machineInfo)
        {
            return Repository.SetMachineInfo(machineInfo);
        }

        #endregion
    }
}