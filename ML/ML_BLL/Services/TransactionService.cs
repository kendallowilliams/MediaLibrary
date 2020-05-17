using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using static MediaLibraryDAL.Enums;
using System.Linq.Expressions;
using MediaLibraryDAL.DbContexts;
using System.Configuration;

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

        public async Task<Transaction> GetNewTransaction(TransactionTypes transactionType)
        {
            Transaction transaction = new Transaction(transactionType);

            transaction.Status = (int)TransactionStatus.Started;
            transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
            await dataService.Insert(transaction);

            return transaction;
        }

        public async Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = null)
        {
            if (transaction != null)
            {
                transaction.Status = (int)TransactionStatus.Completed;
                transaction.StatusMessage = statusMessage ?? $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await dataService.Update(transaction);
            }
        }

        public async Task UpdateTransactionInProcess(Transaction transaction)
        {
            if (transaction != null)
            {
                transaction.Status = (int)TransactionStatus.InProcess;
                transaction.StatusMessage = $"{transaction.Status} [{transaction.Type}]";
                transaction.ModifyDate = DateTime.Now;
                await dataService.Update(transaction);
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
                await dataService.Update(transaction);
            }
        }

        public async Task<Transaction> GetActiveTransactionByType(TransactionTypes transactionType) =>
            await dataService.Get<Transaction>(t => t.Type == (int)transactionType && t.Status == (int)TransactionStatus.InProcess);

        public async Task CleanUpTransactions()
        {
            int.TryParse(ConfigurationManager.AppSettings["TransactionExpirationAge"], out int transactionExpirationDays);
            DateTime expirationDate = DateTime.Now.Date.AddDays(-transactionExpirationDays);
            await dataService.DeleteAll<Transaction>(transaction => transaction.CreateDate < expirationDate);
        }
    }
}