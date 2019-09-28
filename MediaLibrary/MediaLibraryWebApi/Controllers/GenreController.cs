﻿using MediaLibraryBLL.Services.Interfaces;
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
    public class GenreController : ApiControllerBase
    {
        private readonly IGenreService genreService;
        private readonly Genre unknownGenre;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public GenreController(IGenreService genreService, ITransactionService transactionService, IDataService dataService)
        {
            this.genreService = genreService;
            this.transactionService = transactionService;
            this.dataService = dataService;
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
                genres = await dataService.GetList<Genre>();
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
                genre = id > -1 ? await dataService.GetAsync<Genre>(item => item.Id == id): unknownGenre;
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
