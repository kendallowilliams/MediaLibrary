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
using Newtonsoft.Json;
using static MediaLibraryWebUI.Enums;

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
            return await Sort(SongSort.AtoZ);
        }

        [CompressContent]
        public async Task<ActionResult> Sort(SongSort songSort = SongSort.AtoZ, AlbumSort albumSort = AlbumSort.AtoZ, ArtistSort artistSort = ArtistSort.AtoZ)
        {
            musicViewModel.SongGroups = await musicService.GetSongGroups(songSort);
            musicViewModel.ArtistGroups = await musicService.GetArtistGroups(artistSort);
            musicViewModel.AlbumGroups = await musicService.GetAlbumGroups(albumSort);
            musicViewModel.Albums = await musicService.Albums();
            musicViewModel.Artists = await musicService.Artists();
            musicViewModel.Songs = await musicService.Songs();
            musicViewModel.Playlists = await dataService.GetList<Playlist>();
            musicViewModel.SelectedSongSort = songSort;

            return View("Index", musicViewModel);
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
            Transaction transaction = null;
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Accepted);
            string message = string.Empty;

            try
            {
                Transaction existingTransaction = transactionService.GetActiveTransactionByType(TransactionTypes.Read);

                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);

                if (request.IsValid())
                {
                    TrackPath path = await dataService.GetAsync<TrackPath>(item => item.Location.Equals(request.Path, StringComparison.CurrentCultureIgnoreCase));

                    if (path != null)
                    {
                        message = $"'{path.Location}' has already been added. Run {nameof(MusicController)} -> {nameof(Refresh)} instead.";
                        result = new HttpStatusCodeResult(HttpStatusCode.Conflict, message);
                        await transactionService.UpdateTransactionCompleted(transaction, message);
                    }
                    else if (existingTransaction == null)
                    {
                        await controllerService.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, request.Path, request.Recursive, request.Copy), transaction);
                    }
                    else
                    {
                        message = $"{nameof(TransactionTypes.Read)} is already running. See transaction #{existingTransaction.Id}";
                        result = new HttpStatusCodeResult(HttpStatusCode.Conflict, message);
                        await transactionService.UpdateTransactionCompleted(transaction, message);
                    }
                }
                else
                {
                    message = $"{nameof(HttpStatusCode.BadRequest)}: {JsonConvert.SerializeObject(request)}";
                    result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                    await transactionService.UpdateTransactionErrored(transaction, new Exception(message));
                }
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }

        public async Task<ActionResult> Refresh()
        {
            Transaction transaction = null;
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Accepted);

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RefreshMusic);
                await controllerService.QueueBackgroundWorkItem(ct => fileService.CheckForMusicUpdates(transaction), transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }
    }
}