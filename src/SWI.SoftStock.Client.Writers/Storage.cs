using SWI.SoftStock.Client.Common;

namespace SWI.SoftStock.Client.Storages
{
    public abstract class Storage
    {
        protected IRepository Repository { get; set; }

        protected Storage(IRepository repository)
        {
            Repository = repository;
        }
    }
}
