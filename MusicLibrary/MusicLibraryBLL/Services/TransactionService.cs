using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

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
    }
}