using log4net;
using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Client.Common.Options;
using SWI.SoftStock.Client.Facades;
using SWI.SoftStock.Client.ProcessWatchers;
using SWI.SoftStock.Client.Repositories;
using SWI.SoftStock.Client.Storages;
using System;
using System.Configuration;
using System.Threading;

namespace SWI.SoftStock.Client.WindowsService
{
    public class MonitorStarter
    {
        private static Shell shell;

        private static ILog log;

        private static Thread processWatcherThread;

        private static DataMonitor dataMonitor;

        private static readonly object lockobject = new object();

        private static StopToken stopToken;

        private static Shell GetShell(ILog log)
        {
            var mainInfoFacade = new MainInfoFacade();
            var localRepository = new FileRepository();
            mainInfoFacade.LocalStorage = new LocalStorage(localRepository);
            var remoteRepository = new RestRepository(log, GetOption());
            ISoftwareProcessor softwareProcessor = new SoftwareProcessor();
            var watcher = new ProcessWatcher();
            var result = new Shell(mainInfoFacade, localRepository, remoteRepository, softwareProcessor, watcher);
            return result;
        }

        private static RestRepositoryOptions GetOption()
        {
            var option = new RestRepositoryOptions { BaseAddress = ConfigurationManager.AppSettings["BaseAddress"], ApiVersion = ConfigurationManager.AppSettings["ApiVersion"] };
            return option;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StartMonitor(object token)
        {
            stopToken = (StopToken)token;
            if (log == null)
            {
                log4net.Config.BasicConfigurator.Configure();
                log = LogManager.GetLogger(typeof(MonitorStarter));
            }
            if (shell == null)
            {
                shell = GetShell(log);
            }
            log.Info("Start monitor");
            if (dataMonitor == null)
            {
                log.Info("New dataMonitor");
                dataMonitor = new DataMonitor(shell, log, processWatcherThread);
            }

            try
            {
                lock (lockobject)
                {
                    log.Info("Before do work");
                    var workStatus = dataMonitor.DoWork();
                    if (workStatus == WorkStatus.StopService)
                    {
                        log.Info($"Work stopped. workStatus:{workStatus}");
                        stopToken.OnStop();
                    }

                    if (workStatus == WorkStatus.Ok)
                    {
                        stopToken.OnChange(dataMonitor.Period);
                    }

                    log.Info($"Work is done. workStatus:{workStatus} period:{dataMonitor.Period}");
                }
            }
            catch (Exception e)
            {
                log.Error("Error monitor");
                dataMonitor.RaiseError(e);
            }
            log.Info("Stop monitor");
        }

        public static void StopMonitor()
        {
            processWatcherThread?.Abort();
            shell?.Watcher?.Stop();
        }
    }
}