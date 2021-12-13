using System;
using System.Collections.Generic;

namespace SWI.SoftStock.Client.Common
{
    public interface IProcessWatcher
    {
        IEnumerable<Tuple<Guid, string>> Processes { get; set; }
        void Start();
        void Stop();
        event EventHandler<ProcessEventArgs> ProcessStarted;
        event EventHandler<ProcessEventArgs> ProcessStopped;
    }
}