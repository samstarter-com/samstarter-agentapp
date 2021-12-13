namespace SWI.SoftStock.Client.Common
{
    using System;
    using System.Collections.Generic;
    using SWI.SoftStock.Common.Dto;

    public interface IRepository
    {
        Response SetMachineInfo(MachineDto machineInfo);

        Response SetMachineId(Guid machineId);

        void SetCompanyId(Guid machineId);

        MachineDto GetMachineInfo();

        Guid GetMachineId();

        Guid GetCompanyId();

        IList<SoftwareDto> GetSoftwareInfos();

        Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos);

        Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> softwareInfos);

        Response SetProcess(ProcessDto process);

        DataResponse GetData(Guid machineId);

        void RemoveAll();

        Response SetOperationSystem(Guid machineId, OperationSystemDto operationSystem);

        OperationSystemDto GetOperationSystem();

        void SetOperationSystemId(Guid operationSystemId);

        Guid GetOperationSystemId();

        OperationModeDto GetOperationMode();

        Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto operationMode);

        UserDto GetUser();

        Response SetUser(Guid machineId, UserDto user);

        Response SetActivity(Guid machineId);
    }
}