using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryWebApi.Services.Interfaces
{
    public interface IControllerService
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, Transaction transaction);
        void QueueBackgroundWorkItem(Action<CancellationToken> workItem, Transaction transaction);
    }
}
