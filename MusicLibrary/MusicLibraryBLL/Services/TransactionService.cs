using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITransactionService))]
    public class TransactionService : ITransactionService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public TransactionService()
        { }

        public async Task<IEnumerable<Transaction>> GetTransactions() => await dataService.GetList<Transaction>();

        public async Task<Transaction> GetTransaction(object id) => await dataService.Get<Transaction>(id);

        public async Task<int> InsertTransaction(Transaction transaction) => await dataService.Insert<Transaction,int>(transaction);

        public async Task<bool> UpdateTransaction(Transaction transaction) => await dataService.Update(transaction);

        public async Task<Transaction> GetNewTransaction(TransactionTypes transactionType)
        {
            Transaction transaction = new Transaction(transactionType);

            transaction.Status = TransactionStatus.Started;
            transaction.Id = await InsertTransaction(transaction);

            return transaction;
        }

        public async Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = "")
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.Completed;
                transaction.StatusMessage = statusMessage;
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionInProcess(Transaction transaction)
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.InProcess;
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }

        public async Task UpdateTransactionErrored(Transaction transaction, Exception exception)
        {
            if (transaction != null)
            {
                transaction.Status = TransactionStatus.Errored;
                transaction.ErrorMessage = exception.Message;
                transaction.ModifyDate = DateTime.Now;
                await UpdateTransaction(transaction);
            }
        }
    }
}