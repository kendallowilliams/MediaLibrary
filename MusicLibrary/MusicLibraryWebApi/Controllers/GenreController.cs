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
    public class GenreController : ApiController
    {
        private readonly IGenreService genreService;
        private readonly ITransactionService transactionService;
        private readonly Genre unknownGenre;

        [ImportingConstructor]
        public GenreController(IGenreService genreService, ITransactionService transactionService)
        {
            this.genreService = genreService;
            this.transactionService = transactionService;
            this.unknownGenre = new Genre(-1, "Unknown Genre");
        }

        // GET: api/Genre
        public async Task<IEnumerable<Genre>> Get()
        {
            IEnumerable<Genre> genres = Enumerable.Empty<Genre>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetGenres);
                genres = await genreService.GetGenres();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return genres.OrderBy(genre => genre.Name).Concat(Enumerable.Repeat(unknownGenre, 1));
        }

        // GET: api/Genre/5
        public async Task<Genre> Get(int id)
        {
            Genre genre = null;
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetGenre);
                genre = id > -1 ? await genreService.GetGenre(id): unknownGenre;
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return genre;
        }
    }
}
