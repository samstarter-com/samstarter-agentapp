using log4net;

namespace SWI.SoftStock.Client.WindowsService
{
    using Common;
    using SWI.SoftStock.Common.Dto;
    using System;
    using System.Linq;
    using System.Threading;

    public class DataMonitor
    {
        private readonly ILog log;

        private readonly Shell shell;

        private Thread processWatcherThread;

        public DataMonitor(Shell shell, ILog log, Thread processWatcherThread)
        {
            this.shell = shell;
            this.log = log;
            this.processWatcherThread = processWatcherThread;
        }

        public int Period { get; set; }

        public WorkStatus DoWork()
        {
            var status = WorkStatus.Ok;
            try
            {
                status = this.MonitorMachine();
                if (status == WorkStatus.StopService || status == WorkStatus.StopDataCollection)
                {
                    return status;
                }

                this.SetProcesses();
                this.MonitorSoftware();
                this.MonitorOperationSystem();
                this.MonitorOperationMode();
                this.MonitorUser();
                if (this.processWatcherThread == null || this.processWatcherThread.ThreadState == ThreadState.Unstarted)
                {
                    this.processWatcherThread = new Thread(this.StartWatcher);
                    this.processWatcherThread.Start(this.shell.Watcher);
                }
            }
            catch (Exception e)
            {
                this.RaiseError(e);
            }
            return status;
        }

        private WorkStatus MonitorMachine()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            MachineDto currentMachineInfo;
            try
            {
                currentMachineInfo = this.shell.MainInfoFacade.GetMachine();
            }
            catch (Exception e)
            {
                this.RaiseError(e);
                return WorkStatus.Ok;
            }

            if (machineId == Guid.Empty)
            {
                if (currentMachineInfo.CompanyUniqueId == Guid.Empty)
                {
                    this.Info("Company unique id is empty");
                    return WorkStatus.StopService;
                }

                var response = this.shell.RemoteStorage.SetMachineInfo(currentMachineInfo);
                if (response.Code == 0 && response.UniqueId != Guid.Empty)
                {
                    machineId = response.UniqueId;
                    currentMachineInfo.UniqueId = machineId;
                    var machineInfoResponse = this.shell.LocalStorage.SetMachineInfo(currentMachineInfo);
                    if (machineInfoResponse.Code == 0)
                    {
                        this.shell.LocalStorage.SetMachineId(machineId);
                    }
                    if (machineInfoResponse.Code == 18 || machineInfoResponse.Code == 20)
                    {
                        this.Info($"Machine code:{machineInfoResponse.Code}");
                        return WorkStatus.StopService;
                    }
                    if (machineInfoResponse.Code == 21 || machineInfoResponse.Code == 22)
                    {
                        this.Info($"Machine code:{machineInfoResponse.Code}");
                        return WorkStatus.StopDataCollection;
                    }
                }
            }
            else
            {
                var storedMachineInfo = this.shell.LocalStorage.GetMachineInfo();

                if ((((storedMachineInfo != null)) && (!storedMachineInfo.Equals(currentMachineInfo)))
                    || ((storedMachineInfo == null) && (currentMachineInfo != null)))
                {
                    var machineInfoResponse = this.shell.RemoteStorage.SetMachineInfo(currentMachineInfo);
                    if (machineInfoResponse.Code == 0)
                    {
                        this.shell.LocalStorage.SetMachineInfo(currentMachineInfo);
                    }
                    if (machineInfoResponse.Code == 18 || machineInfoResponse.Code == 20)
                    {
                        return WorkStatus.StopService;
                    }
                    if (machineInfoResponse.Code == 21 || machineInfoResponse.Code == 22)
                    {
                        return WorkStatus.StopDataCollection;
                    }
                }
                else
                {
                    var activityResponse = this.shell.RemoteStorage.SetActivity(machineId);
                    if (activityResponse.Code == 18 || activityResponse.Code == 20)
                    {
                        return WorkStatus.StopService;
                    }
                    if (activityResponse.Code == 21)
                    {
                        return WorkStatus.StopDataCollection;
                    }
                }
            }
            return WorkStatus.Ok;
        }

        private void SetProcesses()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var response = this.shell.RemoteStorage.GetData(machineId);
                if (response.Code == 0)
                {
                    this.shell.Watcher.Processes = response.ObservedProcesses.Select(op => new Tuple<Guid, string>(op.Id, op.ProcessName));
                    this.Period = response.AgentData.Interval;
                    this.Info(
                        $"Watched processes:{string.Join(",", this.shell.Watcher.Processes.Select(p => $"id:{p.Item1} name:{p.Item2}"))}");
                }
                else
                {
                    this.Info("No watched process");
                }
            }
        }

        private void MonitorSoftware()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var storedSoftwareInfos = this.shell.LocalStorage.GetSoftwareInfos();
                var currentSoftwareInfos = this.shell.MainInfoFacade.GetSoftwareInfos();
                var modifiedSoftwareInfos = this.shell.SoftwareProcessor.GetModifiedSoftwareInfos(storedSoftwareInfos, currentSoftwareInfos);
                if (modifiedSoftwareInfos.Any())
                {
                    var response = this.shell.RemoteStorage.SetModifiedSoftwareInfos(machineId, modifiedSoftwareInfos);
                    if (response.Code == 0)
                    {
                        this.shell.LocalStorage.SetSoftwareInfos(currentSoftwareInfos);
                    }
                }
            }
        }

        private void OnProcessStarted(object sender, ProcessEventArgs e)
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var request = new ProcessDto();
                request.Name = e.ProcessName;
                request.DateTime = DateTime.UtcNow;
                request.Status = ProcessStatus.Started;
                request.MachineObservableUniqueId = e.ProcessId;
                this.shell.RemoteStorage.SetProcess(request);
            }
        }

        private void OnProcessStoped(object sender, ProcessEventArgs e)
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var request = new ProcessDto();
                request.Name = e.ProcessName;
                request.DateTime = DateTime.UtcNow;
                request.Status = ProcessStatus.Stoped;
                request.MachineObservableUniqueId = e.ProcessId;
                this.shell.RemoteStorage.SetProcess(request);
            }
        }

        private void MonitorOperationSystem()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var storedOperationSystem = this.shell.LocalStorage.GetOperationSystem();
                var currentOperationSystem = this.shell.MainInfoFacade.GetOperationSystem(this.RaiseError);

                if ((((storedOperationSystem != null)) && (!storedOperationSystem.Equals(currentOperationSystem)))
                    || ((storedOperationSystem == null) && (currentOperationSystem != null)))
                {
                    var response = this.shell.RemoteStorage.SetOperationSystem(machineId, currentOperationSystem);

                    if (response.Code == 0 && response.UniqueId != Guid.Empty)
                    {
                        var operationSystemId = response.UniqueId;
                        currentOperationSystem.UniqueId = operationSystemId;
                        var operationSystemResponse = this.shell.LocalStorage.SetOperationSystem(machineId,
                            currentOperationSystem);
                        if (operationSystemResponse.Code == 0)
                        {
                            this.shell.LocalStorage.SetOperationSystemId(operationSystemId);
                        }
                    }
                }
            }
        }

        private void MonitorUser()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            if (machineId != Guid.Empty)
            {
                var currentUser = this.shell.MainInfoFacade.GetUser(this.RaiseError, this.Info);
                var response = this.shell.RemoteStorage.SetUser(machineId, currentUser);
                if (response.Code == 0)
                {
                    this.shell.LocalStorage.SetUser(machineId, currentUser);
                }
            }
        }

        private void MonitorOperationMode()
        {
            var machineId = this.shell.LocalStorage.GetMachineId();
            var operationSystemId = this.shell.LocalStorage.GetOperationSystemId();

            if (machineId != Guid.Empty && operationSystemId != Guid.Empty)
            {
                var storedOperationMode = this.shell.LocalStorage.GetOperationMode();
                var currentOperationMode = this.shell.MainInfoFacade.GetOperationMode();

                if ((((storedOperationMode != null)) && (!storedOperationMode.Equals(currentOperationMode)))
                    || ((storedOperationMode == null) && (currentOperationMode != null)))
                {
                    var response = this.shell.RemoteStorage.SetOperationMode(machineId,
                        operationSystemId,
                        currentOperationMode);

                    if (response.Code == 0)
                    {
                        this.shell.LocalStorage.SetOperationMode(machineId, operationSystemId, currentOperationMode);
                    }
                }
            }
        }

        private void StartWatcher(object watcher)
        {
            var watcher1 = (IProcessWatcher)watcher;
            watcher1.ProcessStarted += this.OnProcessStarted;
            watcher1.ProcessStopped += this.OnProcessStoped;
            watcher1.Start();
        }

        public void RaiseError(Exception e)
        {
            this.log.Error(e);
        }

        private void Info(string message)
        {
            this.log.Info(message);
        }
    }
}