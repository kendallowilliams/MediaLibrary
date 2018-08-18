using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace MusicLibraryWebApi.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        protected ITransactionService transactionService;

        public ApiControllerBase() { }

        public async void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, Transaction transaction)
        {
            await transactionService.UpdateTransactionInProcess(transaction);
            HostingEnvironment.QueueBackgroundWorkItem(workItem);
        }

        public async void QueueBackgroundWorkItem(Action<CancellationToken> workItem, Transaction transaction)
        {
            await transactionService.UpdateTransactionInProcess(transaction);
            HostingEnvironment.QueueBackgroundWorkItem(workItem);
        }
    }
}