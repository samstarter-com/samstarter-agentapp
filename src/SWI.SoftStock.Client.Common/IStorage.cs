namespace SWI.SoftStock.Client.Common
{
    using System;
    using System.Collections.Generic;
    using SWI.SoftStock.Common.Dto;

    public interface IStorage
    {
        Guid GetMachineId();

        Guid GetCompanyId();

        void SetCompanyId(Guid companyId);

        Response SetMachineInfo(MachineDto machineInfo);

        Response SetMachineId(Guid machineId);

        MachineDto GetMachineInfo();

        IList<SoftwareDto> GetSoftwareInfos();

        Response SetSoftwareInfos(IList<SoftwareDto> softwareInfos);

        Response SetProcess(ProcessDto process);

        Response SetModifiedSoftwareInfos(Guid machineId, IList<SoftwareStatusDto> modifiedSoftwareInfos);

        DataResponse GetData(Guid machineId);

        OperationSystemDto GetOperationSystem();

        Response SetOperationSystem(Guid machineId, OperationSystemDto currentOperationSystem);

        void SetOperationSystemId(Guid operationSystemId);

        Guid GetOperationSystemId();

        OperationModeDto GetOperationMode();

        Response SetOperationMode(Guid machineId, Guid operationSystemId, OperationModeDto currentOperationMode);

        UserDto GetUser();

        Response SetUser(Guid machineId, UserDto currentUser);

        Response SetActivity(Guid machineId);
    }
}