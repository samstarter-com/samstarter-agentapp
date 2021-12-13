namespace SWI.SoftStock.Client.Common
{
    public interface IShell
    {
        IMainInfoFacade MainInfoFacade { get; }
        IStorage LocalStorage { get; }
        IStorage RemoteStorage { get; }
    }
}