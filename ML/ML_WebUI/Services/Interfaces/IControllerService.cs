using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface IControllerService
    {
        Task QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, Transaction transaction);
        Task QueueBackgroundWorkItem(Action<CancellationToken> workItem, Transaction transaction);
    }
}
