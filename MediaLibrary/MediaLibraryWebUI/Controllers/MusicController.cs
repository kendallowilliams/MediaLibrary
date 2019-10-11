using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.ActionResults;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediaLibraryWebUI.Attributes;
using MediaLibraryWebUI.DataContracts;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Music", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MusicController : BaseController
    {
        private readonly IDataService dataService;
        private readonly IMusicUIService musicService;
        private readonly MusicViewModel musicViewModel;
        private readonly ITrackService trackService;
        private readonly IFileService fileService;
        private readonly IControllerService controllerService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public MusicController(IDataService dataService, IMusicUIService musicService, MusicViewModel musicViewModel,
                               ITrackService trackService, IFileService fileService, IControllerService controllerService,
                               ITransactionService transactionService)
        {
            this.dataService = dataService;
            this.musicService = musicService;
            this.musicViewModel = musicViewModel;
            this.trackService = trackService;
            this.fileService = fileService;
            this.controllerService = controllerService;
            this.transactionService = transactionService;
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            musicViewModel.Albums = await musicService.Albums();
            musicViewModel.Artists = await musicService.Artists();
            musicViewModel.Songs = await musicService.Songs();
            musicViewModel.SongGroups = await musicService.GetSongGroups();
            musicViewModel.ArtistGroups = await musicService.GetArtistGroups();
            musicViewModel.AlbumGroups = await musicService.GetAlbumGroups();
            musicViewModel.Playlists = await dataService.GetList<Playlist>();

            return View(musicViewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> File(int id)
        {
            TrackFile file = await trackService.GetTrackFile(id);
            string range = Request.Headers["Range"];
            ActionResult result = null;

            if (file != null)
            {
                result = new RangeFileContentResult(file?.Data, range, file.Type);
            }
            else
            {
                result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task AddTrackToPlaylist(int itemId, int playlistId)
        {
            PlaylistTrack item = new PlaylistTrack() { PlaylistId = playlistId, TrackId = itemId };

            await dataService.Insert(item);
        }

        public async Task AddArtistToPlaylist(int itemId, int playlistId)
        {
            IEnumerable<Track> tracks = await dataService.GetList<Track>(track => track.ArtistId == itemId);
            IEnumerable<PlaylistTrack> items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });

            await dataService.Insert(items);
        }

        public async Task AddAlbumToPlaylist(int itemId, int playlistId)
        {
            IEnumerable<Track> tracks = await dataService.GetList<Track>(track => track.AlbumId == itemId);
            IEnumerable<PlaylistTrack> items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });

            await dataService.Insert(items);
        }

        public async Task<ActionResult> GetAlbum(int id)
        {
            musicViewModel.SelectedAlbum = await dataService.GetAsync<Album, IEnumerable<Track>>(album => album.Id == id, album => album.Tracks);

            return View("Album", musicViewModel);
        }

        public async Task<ActionResult> GetArtist(int id)
        {
            musicViewModel.SelectedArtist = await dataService.GetAsync<Artist, IEnumerable<Album>>(artist => artist.Id == id, artist => artist.Albums);

            return View("Artist", musicViewModel);
        }

        public async Task<ActionResult> Scan(ScanDirectoryRequest request)
        {
            Transaction transaction = await transactionService.GetNewTransaction(TransactionTypes.Read),
                        existingTransaction = transactionService.GetActiveTransactionByType(TransactionTypes.Read);
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Accepted);

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);
                existingTransaction = transactionService.GetActiveTransactionByType(TransactionTypes.Read);

                if (existingTransaction == null)
                {
                    await controllerService.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, request.Directory, request.Recursive, request.Copy), transaction);
                }
                else
                {
                    result = new HttpStatusCodeResult(HttpStatusCode.Conflict,
                                                      $"{nameof(TransactionTypes.Read)} is already running. See transaction #{existingTransaction.Id}");
                }
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }
    }
}