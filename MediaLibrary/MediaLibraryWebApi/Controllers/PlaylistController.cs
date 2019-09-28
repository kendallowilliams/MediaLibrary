using MediaLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static MediaLibraryDAL.Enums.TransactionEnums;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;

namespace MediaLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : ApiControllerBase
    {
        private readonly IPlaylistService playlistService;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PlaylistController(IPlaylistService playlistService, ITransactionService transactionService, IDataService dataService)
        {
            this.playlistService = playlistService;
            this.transactionService = transactionService;
            this.dataService = dataService;
        }

        // GET: api/Playlist
        public async Task<IEnumerable<Playlist>> Get()
        {
            IEnumerable<Playlist> playlists = Enumerable.Empty<Playlist>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPlaylists);
                playlists = await dataService.GetList<Playlist>();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
            
            return playlists.OrderBy(playlist => playlist.Name);
        }

        // GET: api/Playlist/5
        public async Task<Playlist> Get(int id)
        {
            Transaction transaction = null;
            Playlist playlist = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPlaylist);
                playlist = await dataService.GetAsync<Playlist>(item => item.Id == id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return playlist;
        }

        // POST: api/Playlist
        public async Task<int> Post([FromBody]Playlist playlist)
        {
            Transaction transaction = null;
            int playlistId = -1;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylist);
                await dataService.Insert(playlist);
                playlistId = playlist.Id;
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return playlistId;
        }

        // PUT: api/Playlist/5
        public async Task<bool> Put(int id, [FromBody]Playlist playlist)
        {
            Transaction transaction = null;
            bool isReplaced = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.ReplacePlaylist);
                isReplaced = await dataService.Update(playlist) > 0;
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isReplaced;
        }

        // DELETE: api/Playlist/5
        public async Task Delete(int id)
        {
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RemovePlaylist);
                await dataService.Delete<Playlist>(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }
    }
}
