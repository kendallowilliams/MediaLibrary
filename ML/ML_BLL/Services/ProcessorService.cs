using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.DbContexts;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IProcessorService))]
    public class ProcessorService : IProcessorService
    {
        private readonly IDataService dataService;
        private readonly IPodcastService podcastService;
        private readonly IFileService fileService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public ProcessorService(IDataService dataService, IPodcastService podcastService, IFileService fileService,
                                ITransactionService transactionService)
        {
            this.dataService = dataService;
            this.podcastService = podcastService;
            this.fileService = fileService;
            this.transactionService = transactionService;
        }

        public async Task RefreshPodcasts()
        {
            Transaction transaction = null;
            IEnumerable<Podcast> podcasts = null;
            IEnumerable<Task<Podcast>> tasks = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RefreshPodcast);
                podcasts = await dataService.GetList<Podcast>();
                tasks = podcasts.Select(podcast => podcastService.RefreshPodcast(podcast));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task RefreshMusic()
        {
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RefreshMusic);
                await fileService.CheckForMusicUpdates(transaction);
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task PerformCleanup()
        {
            await transactionService.CleanUpTransactions();
        }
    }
}