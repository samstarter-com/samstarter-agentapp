using System;

namespace SWI.SoftStock.Client.WindowsService
{
    public class StopToken
    {
        // An event that clients can use to be notified whenever the
        // elements of the list change:
        public event EventHandler Stop;

        // An event that clients can use to be notified whenever the
        // elements of the list change:
        public event EventHandler<ChangeEventArgs> Change;

        // Invoke the Changed event; called whenever list changes:
        public virtual void OnStop()
        {
            Stop?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">timer period in ms</param>
        public virtual void OnChange(int period)
        {
            Change?.Invoke(this, new ChangeEventArgs() { Period = period });
        }
    }

    public class ChangeEventArgs : EventArgs
    {
        public int Period { get; set; }
    }
}