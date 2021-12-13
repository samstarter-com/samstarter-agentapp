using System;
using System.Collections.Generic;
using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Common.Dto;

namespace SWI.SoftStock.Client.Storages
{
    public class LocalStorage : Storage, IStorage
    {
        public LocalStorage(IRepository repository) : base(repository)
        {
        }

        #region IStorage Members

        public Guid GetMachineId()
        {
            return Repository.GetMachineId();
        }

        public Guid GetCompanyId()
        {
            return Repository.GetCompanyId();
        }

        public void SetCompanyId(Guid companyId)
        {
            Repository.SetCompanyId(companyId);
        }

        public Response SetMachineId(Guid machineId)
        {
            return Repository.SetMachineId(machineId);
        }

        public MachineDto GetMachineInfo()
        {
            return Repository.GetMachineInfo();
        }

        public IList<SoftwareDto> GetSoftwareInfos()
        {
            return Repository.GetSoftwareInfos();
        }

        public Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos)
        {
            return Repository.SetSoftwareInfos(softwareInfos);
        }

        public Response SetProcess(ProcessDto process)
        {
            throw new NotImplementedException();
        }

        public Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> modifiedSoftwareInfos)
        {
            return Repository.SetModifiedSoftwareInfos(machineId, modifiedSoftwareInfos);
        }

        public DataResponse GetData(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public OperationSystemDto GetOperationSystem()
        {
            return Repository.GetOperationSystem();
        }

        public Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem)
        {
            return Repository.SetOperationSystem(machineId, operationSystem);
        }

        public void SetOperationSystemId(Guid operationSystemId)
        {
            Repository.SetOperationSystemId(operationSystemId);
        }

        public Guid GetOperationSystemId()
        {
            return Repository.GetOperationSystemId();
        }

        public OperationModeDto GetOperationMode()
        {
            return Repository.GetOperationMode();
        }

        public Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto currentOperationMode)
        {
            return Repository.SetOperationMode(machineId, operationSystemId, currentOperationMode);
        }

        public UserDto GetUser()
        {
            return Repository.GetUser();
        }

        public Response SetUser(Guid machineId, UserDto user)
        {
            return Repository.SetUser(machineId, user);
        }

        public Response SetActivity(Guid machineId)
        {
            throw new NotImplementedException();
        }

        public Response SetMachineInfo(MachineDto machineInfo)
        {
            return Repository.SetMachineInfo(machineInfo);
        }

        #endregion

        public void RemoveAll()
        {
            Repository.RemoveAll();
        }
    }
}