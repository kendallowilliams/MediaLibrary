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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : ApiControllerBase
    {
        private readonly IPlaylistService playlistService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public PlaylistController(IPlaylistService playlistService, ITransactionService transactionService)
        {
            this.playlistService = playlistService;
            this.transactionService = transactionService;
        }

        // GET: api/Playlist
        public async Task<IEnumerable<Playlist>> Get()
        {
            IEnumerable<Playlist> playlists = Enumerable.Empty<Playlist>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetPlaylists);
                playlists = await playlistService.GetPlaylists();
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
                playlist = await playlistService.GetPlaylist(id);
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
                playlistId = await playlistService.InsertPlaylist(playlist);
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
                isReplaced = await playlistService.UpdatePlaylist(playlist);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return isReplaced;
        }

        // DELETE: api/Playlist/5
        public async Task<bool> Delete(int id)
        {
            Transaction transaction = null;
            bool isRemoved = false;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RemovePlaylist);
                isRemoved = await playlistService.DeletePlaylist(id);
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
