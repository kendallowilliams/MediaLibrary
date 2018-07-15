using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetTransaction(object id);

        Task<IEnumerable<Transaction>> GetTransactions(object predicate = null);

        Task<int> InsertTransaction(Transaction transaction);

        Task<bool> UpdateTransaction(Transaction transaction);

        Task<Transaction> GetNewTransaction(TransactionTypes transactionType);

        Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = null);

        Task UpdateTransactionInProcess(Transaction transaction);

        Task UpdateTransactionErrored(Transaction transaction, Exception exception);

        Task<Transaction> GetActiveTransactionByType(TransactionTypes transactionType);
    }
}
