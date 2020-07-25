using System;

namespace EAPN.HDVS.Application.Core.Services
{
    public abstract class ServiceBase : IServiceBase
    {
        protected abstract void ReleaseManagedResources();

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
