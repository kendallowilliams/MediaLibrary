using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.Models;
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

        public IEnumerable<Transaction> GetTransactions(Expression<Func<Transaction,bool>> expression = null) => dataService.GetList(expression);

        public Transaction GetTransaction(Expression<Func<Transaction, bool>> expression = null) => dataService.Get(expression);

        public async Task<int> InsertTransaction(Transaction transaction) => await dataService.Insert(transaction);

        public async Task<int> UpdateTransaction(Transaction transaction) => await dataService.Update(transaction);

        public async Task<Transaction> GetNewTransaction(TransactionTypes transactionType)
        {
            Transaction transaction = new Transaction(transactionType);

            transaction.Status = TransactionStatus.Started;
            transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
            transaction.Id = await InsertTransaction(transaction);

            return transaction;
        }

        public async Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = null)
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.Completed;
                transaction.StatusMessage = statusMessage ?? $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionInProcess(Transaction transaction)
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.InProcess;
                transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionErrored(Transaction transaction, Exception exception)
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.Errored;
                transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
                transaction.ErrorMessage = exception.Message;
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public Transaction GetActiveTransactionByType(TransactionTypes transactionType)
        {
            Transaction transaction = default(Transaction);

            using (var db = new MediaLibraryContext())
            {
                transaction = (from x in db.Tranactions
                              where x.Type == transactionType && x.Status == TransactionStatus.InProcess
                              select x)
                              .FirstOrDefault();
            }

            return transaction;
        }
    }
}