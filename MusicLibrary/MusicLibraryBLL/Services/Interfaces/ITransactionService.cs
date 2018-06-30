using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetTransaction(object id);

        Task<IEnumerable<Transaction>> GetTransactions();

        Task<int> InsertTransaction(Transaction transaction);

        Task<bool> UpdateTransaction(Transaction transaction);
    }
}
