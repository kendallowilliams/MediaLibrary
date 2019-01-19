using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MusicLibraryWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace MusicLibraryWebApi.Services
{
    [Export(typeof(IControllerService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class ControllerService : IControllerService
    {
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public ControllerService(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, Transaction transaction)
        {
            transactionService.UpdateTransactionInProcess(transaction).Wait();
            HostingEnvironment.QueueBackgroundWorkItem(workItem);
        }

        public void QueueBackgroundWorkItem(Action<CancellationToken> workItem, Transaction transaction)
        {
            transactionService.UpdateTransactionInProcess(transaction).Wait();
            HostingEnvironment.QueueBackgroundWorkItem(workItem);
        }
    }
}