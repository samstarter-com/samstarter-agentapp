using log4net;
using System;
using System.ServiceProcess;

namespace SWI.SoftStock.Client.WindowsService
{
    static class Program
    {
        private static ILog log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = LogManager.GetLogger(typeof(Program));

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var servicesToRun = new ServiceBase[]
            {
                new SoftStock()
            };
            ServiceBase.Run(servicesToRun);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            log.Error(exception);
        }
    }
}
