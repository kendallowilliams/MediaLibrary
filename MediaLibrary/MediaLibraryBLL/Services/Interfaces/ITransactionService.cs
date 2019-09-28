using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetNewTransaction(TransactionTypes transactionType);

        Task UpdateTransactionCompleted(Transaction transaction, string statusMessage = null);

        Task UpdateTransactionInProcess(Transaction transaction);

        Task UpdateTransactionErrored(Transaction transaction, Exception exception);

        Transaction GetActiveTransactionByType(TransactionTypes transactionType);
    }
}
