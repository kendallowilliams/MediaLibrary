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

namespace MediaLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ArtistController : ApiControllerBase
    {
        private readonly IArtistService artistService;
        private readonly Artist unknownArtist;

        [ImportingConstructor]
        public ArtistController(IArtistService artistService, ITransactionService transactionService)
        {
            this.artistService = artistService;
            this.transactionService = transactionService;
            this.unknownArtist = new Artist(-1, "Unknown Artist");
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

            return artists.Concat(Enumerable.Repeat(unknownArtist, 1)).OrderBy(artist => artist.Name);
        }

        // GET: api/Artist/5
        public async Task<Artist> Get(int id)
        {
            Artist artist = null;
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetArtist);
                artist = id > -1 ? await artistService.GetArtist(item => item.Id == id) : unknownArtist;
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
