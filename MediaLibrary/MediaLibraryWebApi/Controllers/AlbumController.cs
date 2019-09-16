using MediaLibraryDAL.Models;
using MediaLibraryBLL.Services;
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

namespace MediaLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlbumController : ApiControllerBase
    {
        private readonly IAlbumService albumService;
        private readonly Album unknownAlbum;

        [ImportingConstructor]
        public AlbumController(IAlbumService albumService, ITransactionService transactionService)
        {
            this.albumService = albumService;
            this.transactionService = transactionService;
            unknownAlbum = new Album(-1, "Unknown Album");
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

            return albums.Concat(Enumerable.Repeat(unknownAlbum, 1)).OrderBy(album => album.Title);
        }

        // GET: api/Album/5
        public async Task<Album> Get(int id)
        {
            Transaction transaction = null;
            Album album = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetAlbum);
                album = id > -1 ? await albumService.GetAlbum(item => item.Id == id): unknownAlbum;
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
