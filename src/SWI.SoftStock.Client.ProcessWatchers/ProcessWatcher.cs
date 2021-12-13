using SWI.SoftStock.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace SWI.SoftStock.Client.ProcessWatchers
{
    public class ProcessWatcher : IProcessWatcher
    {
        private ManagementEventWatcher mgmtStartedWtch;

        private ManagementEventWatcher mgmtStopedWtch;

        #region IProcessWatcher Members

        public IEnumerable<Tuple<Guid, string>> Processes { get; set; }

        public void Start()
        {
            this.mgmtStartedWtch = new ManagementEventWatcher("Select * From Win32_ProcessStartTrace");
            this.mgmtStartedWtch.EventArrived += MgmtStartedWtchEventArrived;
            this.mgmtStartedWtch.Start();

            this.mgmtStopedWtch = new ManagementEventWatcher("Select * FROM Win32_ProcessStopTrace");
            this.mgmtStopedWtch.EventArrived += MgmtStopedWtchEventArrived;
            this.mgmtStopedWtch.Start();
        }

        public void Stop()
        {
            this.mgmtStartedWtch?.Stop();
            this.mgmtStopedWtch?.Stop();
        }

        public event EventHandler<ProcessEventArgs> ProcessStarted;
        public event EventHandler<ProcessEventArgs> ProcessStopped;

        #endregion

        private void MgmtStartedWtchEventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = (string)e.NewEvent["ProcessName"];
            var prs = Processes.Where(p => FindFilesPatternToRegex.Convert(p.Item2).IsMatch(processName));
            var enumerable = prs as Tuple<Guid, string>[] ?? prs.ToArray();
            if (enumerable.Any())
            {
                ProcessStarted?.Invoke(this, new ProcessEventArgs(processName, enumerable.First().Item1));
            }
        }

        private void MgmtStopedWtchEventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = (string)e.NewEvent["ProcessName"];
            var prs = Processes.Where(p => FindFilesPatternToRegex.Convert(p.Item2).IsMatch(processName));
            var enumerable = prs as Tuple<Guid, string>[] ?? prs.ToArray();
            if (enumerable.Any())
            {  
                ProcessStopped?.Invoke(this, new ProcessEventArgs(processName, enumerable.First().Item1));
            }
        }
    }
}