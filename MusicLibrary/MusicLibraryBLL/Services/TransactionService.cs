using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DapperExtensions;
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
        private readonly IDataService dataService;

        [ImportingConstructor]
        public TransactionService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(object predicate = null) => await dataService.GetList<Transaction>(predicate);

        public async Task<Transaction> GetTransaction(object id) => await dataService.Get<Transaction>(id);

        public async Task<int> InsertTransaction(Transaction transaction) => await dataService.Insert<Transaction,int>(transaction);

        public async Task<bool> UpdateTransaction(Transaction transaction) => await dataService.Update(transaction);

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

        public async Task<Transaction> GetActiveTransactionByType(TransactionTypes transactionType)
        {
            Transaction transaction = null;
            var group = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            group.Predicates.Add(Predicates.Field<Transaction>(t => t.Type, Operator.Eq, transactionType));
            group.Predicates.Add(Predicates.Field<Transaction>(t => t.Status, Operator.Eq, TransactionStatus.InProcess));
            transaction = (await GetTransactions(group)).FirstOrDefault();

            return transaction;
        }
    }
}