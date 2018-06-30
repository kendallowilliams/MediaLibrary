using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services;
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
    public class AlbumController : ApiController
    {
        private readonly IAlbumService albumService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public AlbumController(IAlbumService albumService, ITransactionService transactionService)
        {
            this.albumService = albumService;
            this.transactionService = transactionService;
        }

        // GET: api/Album
        public async Task<IEnumerable<Album>> Get()
        {
            IEnumerable<Album> albums = Enumerable.Empty<Album>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetAlbums);
                albums = await albumService.GetAlbums();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return albums.OrderBy(album => album.Title);
        }

        // GET: api/Album/5
        public async Task<Album> Get(int id)
        {
            Transaction transaction = null;
            Album album = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetAlbum);
                album = await albumService.GetAlbum(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return album;
        }
    }
}
