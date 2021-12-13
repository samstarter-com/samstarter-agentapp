using SWI.SoftStock.Common.Dto;
using System;
using System.Collections.Generic;

namespace SWI.SoftStock.Client.Common
{
    public interface IMainInfoFacade
    {
        IStorage LocalStorage { get; set; }
        IList<SoftwareDto> GetSoftwareInfos();
        MachineDto GetMachine();
        UserDto GetUser(Action<Exception> raiseError, Action<string> info);
        OperationSystemDto GetOperationSystem(Action<Exception> raiseError);
        OperationModeDto GetOperationMode();
    }
}