using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using MediaLibraryDAL.DbContexts;

namespace MediaLibraryWebUI.Services
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

        public async Task QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, Transaction transaction)
        {
            await transactionService.UpdateTransactionInProcess(transaction);
            HostingEnvironment.QueueBackgroundWorkItem(async ct => await workItem(ct));
        }

        public async Task QueueBackgroundWorkItem(Action<CancellationToken> workItem, Transaction transaction)
        {
            await transactionService.UpdateTransactionInProcess(transaction);
            HostingEnvironment.QueueBackgroundWorkItem(workItem);
        }
    }
}