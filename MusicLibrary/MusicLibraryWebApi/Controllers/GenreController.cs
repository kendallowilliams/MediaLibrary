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

        [ImportingConstructor]
        public GenreController(IGenreService genreService, ITransactionService transactionService)
        {
            this.genreService = genreService;
            this.transactionService = transactionService;
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

            return genres.OrderBy(genre => genre.Name);
        }

        // GET: api/Genre/5
        public async Task<Genre> Get(int id)
        {
            Genre genre = null;
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetGenre);
                genre = await genreService.GetGenre(id);
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
