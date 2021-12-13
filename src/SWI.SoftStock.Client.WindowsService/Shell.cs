using SWI.SoftStock.Client.Common;
using SWI.SoftStock.Client.Storages;

namespace SWI.SoftStock.Client.WindowsService
{
    public class Shell : IShell
    {
        public Shell(
            IMainInfoFacade mainInfoFacade,
            IRepository localRepository, 
            IRepository remoteRepository,
            ISoftwareProcessor softwareProcessor, 
            IProcessWatcher watcher)
        {
            this.MainInfoFacade = mainInfoFacade;
            this.SoftwareProcessor = softwareProcessor;
            this.LocalStorage = new LocalStorage(localRepository);
            this.RemoteStorage = new RemoteStorage(remoteRepository);
            this.Watcher = watcher;
        }

        public ISoftwareProcessor SoftwareProcessor { get; private set; }

        public IProcessWatcher Watcher { get; private set; }

        #region IShell Members

        public IMainInfoFacade MainInfoFacade { get; private set; }
        public IStorage LocalStorage { get; private set; }
        public IStorage RemoteStorage { get; private set; }

        #endregion
    }
}