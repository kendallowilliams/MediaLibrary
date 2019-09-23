using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using static MediaLibraryDAL.Enums.TransactionEnums;
using System.Linq.Expressions;
using MediaLibraryDAL.DbContexts;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITransactionService))]
    public class TransactionService : ITransactionService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public TransactionService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(Expression<Func<Transaction,bool>> expression = null) => await dataService.GetList(expression);

        public async Task<Transaction> GetTransaction(Expression<Func<Transaction, bool>> expression = null) => await dataService.GetAsync(expression);

        public async Task<int> InsertTransaction(Transaction transaction) => await dataService.Insert(transaction);

        public async Task<int> UpdateTransaction(Transaction transaction) => await dataService.Update(transaction);

        public async Task<Transaction> GetNewTransaction(TransactionTypes transactionType)
        {
            Transaction transaction = new Transaction(transactionType);

            transaction.Status = (int)TransactionStatus.Started;
            transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
            await InsertTransaction(transaction);

            return transaction;
        }

        public async Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = null)
        {
            if (transaction != null)
            {
                transaction.Status = (int)TransactionStatus.Completed;
                transaction.StatusMessage = statusMessage ?? $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionInProcess(Transaction transaction)
        {
            if (transaction != null)
            {
                transaction.Status = (int)TransactionStatus.InProcess;
                transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionErrored(Transaction transaction, Exception exception)
        {
            if (transaction != null)
            {
                transaction.Status = (int)TransactionStatus.Errored;
                transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
                transaction.ErrorMessage = exception.Message;
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public Transaction GetActiveTransactionByType(TransactionTypes transactionType)
        {
            Transaction transaction = default(Transaction);

            using (var db = new MediaLibraryEntities())
            {
                transaction = (from x in db.Transactions
                              where x.Type == (int)transactionType && x.Status == (int)TransactionStatus.InProcess
                              select x)
                              .FirstOrDefault();
            }

            return transaction;
        }
    }
}