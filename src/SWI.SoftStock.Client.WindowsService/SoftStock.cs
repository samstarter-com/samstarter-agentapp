using System;
using System.ServiceProcess;
using System.Threading;

namespace SWI.SoftStock.Client.WindowsService
{
    public partial class SoftStock : ServiceBase
    {
        private Thread thread;

        private Timer timer;

        private const int MinPeriod = 5000; // 5s
        private const int MaxPeriod = 3600000; //1h
        /// <summary>
        /// timer period in ms. default value is 1h
        /// </summary>
        private int period = 3600000; 

        public SoftStock()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.thread = new Thread(this.WorkerThreadFunc);
            this.thread.Name = "SamStarter Service Thread";
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        private void WorkerThreadFunc()
        {
            var token=new StopToken();
            token.Stop += this.StopMonitor;
            token.Change += this.ChangeMonitor;
            var tcb = new TimerCallback(MonitorStarter.StartMonitor);
            this.timer = new Timer(tcb, token, 0, period);
        }

        private void ChangeMonitor(object sender, ChangeEventArgs e)
        {
            if (this.period != e.Period &&
                e.Period >= MinPeriod && e.Period <= MaxPeriod)
            {
                this.period = e.Period;
                this.timer.Change(e.Period, e.Period);
            }
        }

        private void StopMonitor(object sender, EventArgs eventArgs)
        {
            this.timer.Dispose();
            this.thread.Abort();
        }
    }
}