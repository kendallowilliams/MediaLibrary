using MediaLibraryDAL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryWebApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class RootController : ApiControllerBase
    {
        private readonly IFileService fileService;
        private readonly IAlbumService albumService;
        private readonly IArtistService artistService;
        private readonly ITrackService trackService;
        private readonly IPodcastService podcastService;
        private readonly IPlaylistService playlistService;
        private readonly IGenreService genreService;
        private readonly IControllerService controllerService;

        [ImportingConstructor]
        public RootController(IFileService fileService, ITransactionService transactionService, IAlbumService albumService,
                              IArtistService artistService, ITrackService trackService, IPodcastService podcastService,
                              IPlaylistService playlistService, IGenreService genreService, IControllerService controllerService)
        {
            this.fileService = fileService;
            this.transactionService = transactionService;
            this.albumService = albumService;
            this.artistService = artistService;
            this.trackService = trackService;
            this.podcastService = podcastService;
            this.playlistService = playlistService;
            this.genreService = genreService;
            this.controllerService = controllerService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Read([FromBody] JObject inData)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);
            Transaction transaction = null,
                        existingTransaction = null;

            try
            {
                string path = inData["path"]?.ToString();
                bool copyFiles = false,
                     recursive = false,
                     validData = !string.IsNullOrWhiteSpace(path) &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles) &&
                                 bool.TryParse(inData["recursive"]?.ToString(), out recursive);
                string responseMessage = $"Invalid Data: [{inData}]",
                       transactionType = Enum.GetName(typeof(TransactionTypes), TransactionTypes.Read);

                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);
                existingTransaction = transactionService.GetActiveTransactionByType(TransactionTypes.Read);

                if (validData && existingTransaction == null)
                {
                    controllerService.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, path, recursive, copyFiles), transaction);
                }
                else
                {
                    if (existingTransaction != null)
                    {
                        responseMessage = $"{transactionType} is already running. See transaction #{existingTransaction.Id}";
                        response = Request.CreateResponse(HttpStatusCode.Conflict, responseMessage);
                    }
                    else if (!validData)
                    {
                        responseMessage = $"Invalid Data: [{inData}]";
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, responseMessage);
                    }

                    await transactionService.UpdateTransactionCompleted(transaction, responseMessage);
                }
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ResetData()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.ResetData);
                await trackService.DeleteAllTracks();
                await playlistService.DeleteAllPlaylists();
                await albumService.DeleteAllAlbums();
                await genreService.DeleteAllGenres();
                await artistService.DeleteAllArtists();
                await podcastService.DeleteAllPodcasts();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
