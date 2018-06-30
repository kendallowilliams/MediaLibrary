using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryWebApi.Controllers
{
    [Export]
    public class PodcastController : ApiController
    {
        private readonly IPodcastService podcastService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService, ITransactionService transactionService)
        {
            this.podcastService = podcastService;
            this.transactionService = transactionService;
        }

        // GET: api/Podcast
        public async Task<IEnumerable<Podcast>> Get()
        {
            IEnumerable<Podcast> podcasts = Enumerable.Empty<Podcast>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcasts);
                podcasts = await podcastService.GetPodcasts();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcasts.OrderBy(podcast => podcast.Title);
        }

        // GET: api/Podcast/5
        public async Task<Podcast> Get(int id)
        {
            Transaction transaction = null;
            Podcast podcast = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPodcast);
                podcast = await podcastService.GetPodcast(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
            
            return podcast;
        }

        // POST: api/Podcast
        public async Task<int> Post([FromBody]Podcast podcast)
        {
            Transaction transaction = null;
            int podcastId = -1;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPodcast);
                podcastId = await podcastService.InsertPodcast(podcast);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcastId;
        }

        // PUT: api/Podcast/5
        public async Task<bool> Put(int id, [FromBody]Podcast podcast)
        {
            Transaction transaction = null;
            bool isReplaced = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.ReplacePodcast);
                isReplaced = await podcastService.UpdatePodcast(podcast);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isReplaced;
        }

        // DELETE: api/Podcast/5
        public async Task<bool> Delete(int id)
        {
            Transaction transaction = null;
            bool isRemoved = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RemovePodcast);
                isRemoved = await podcastService.DeletePodcast(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isRemoved;
        }
    }
}
