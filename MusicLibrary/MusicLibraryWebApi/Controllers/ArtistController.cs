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
    [Export]
    public class ArtistController : ApiController
    {
        private readonly IArtistService artistService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public ArtistController(IArtistService artistService, ITransactionService transactionService)
        {
            this.artistService = artistService;
            this.transactionService = transactionService;
        }

        // GET: api/Artist
        public async Task<IEnumerable<Artist>> Get()
        {
            IEnumerable<Artist> artists = Enumerable.Empty<Artist>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetArtists);
                artists = await artistService.GetArtists();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return artists;
        }

        // GET: api/Artist/5
        public async Task<Artist> Get(int id)
        {
            Artist artist = null;
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetArtist);
                artist = await artistService.GetArtist(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return artist;
        }
    }
}
