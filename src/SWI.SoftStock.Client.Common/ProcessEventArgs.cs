namespace SWI.SoftStock.Client.Common
{
    using System;

    public class ProcessEventArgs : EventArgs
    {
        private readonly Guid processId;

        private readonly string processName;

        public ProcessEventArgs(string processName, Guid processId)
        {
            this.processName = processName;
            this.processId = processId;
        }

        public string ProcessName
        {
            get
            {
                return processName;
            }
        }

        public Guid ProcessId
        {
            get
            {
                return processId;
            }
        }
    }
}