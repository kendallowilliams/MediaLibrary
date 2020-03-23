using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryBLL.Services
{
    [Export(typeof(ILogService))]
    public class LogService : ILogService
    {
        private readonly IDataService dataService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public LogService(IDataService dataService, ITransactionService transactionService)
        {
            this.dataService = dataService;
            this.transactionService = transactionService;
        }

        public async Task Log(TransactionTypes transactionType, string message)
        {
            Transaction transaction = await transactionService.GetNewTransaction(transactionType);

            transaction.Message = message;
            await transactionService.UpdateTransactionCompleted(transaction);
        }
    }
}
