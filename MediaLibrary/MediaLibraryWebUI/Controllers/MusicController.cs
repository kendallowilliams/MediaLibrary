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
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Models.Data;
using System.Web;
using System.IO;

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
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Music));

            if (configuration != null)
            {
                musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            }

            if (musicViewModel.Configuration.SelectedMusicPage == MusicPages.Album)
            {
                result = await GetAlbum(musicViewModel.Configuration.SelectedAlbumId);
            }
            else if (musicViewModel.Configuration.SelectedMusicPage == MusicPages.Artist)
            {
                result = await GetArtist(musicViewModel.Configuration.SelectedArtistId);
            }
            else
            {
                musicViewModel.SongGroups = await musicService.GetSongGroups(musicViewModel.Configuration.SelectedSongSort);
                musicViewModel.ArtistGroups = await musicService.GetArtistGroups(musicViewModel.Configuration.SelectedArtistSort);
                musicViewModel.AlbumGroups = await musicService.GetAlbumGroups(musicViewModel.Configuration.SelectedAlbumSort);
                musicViewModel.Albums = await musicService.Albums();
                musicViewModel.Artists = await musicService.Artists();
                musicViewModel.Songs = await musicService.Songs();
                musicViewModel.Playlists = await dataService.GetList<Playlist>();
                result = View(musicViewModel);
            }

            return result;
        }

        public ActionResult GetSongGroup(string key)
        {
            return PartialView("~/Views/Music/SongGroup.cshtml", musicViewModel.SongGroups.FirstOrDefault(group => group.Key == key));
        }

        [AllowAnonymous]
        public async Task<ActionResult> File(int id)
        {
            TrackFile file = await trackService.GetTrackFile(id);
            string range = Request.Headers["Range"];
            ActionResult result = null;

            if (file != null)
            {
                result = new RangeFileContentResult(file.Data, range, file.Type);
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
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Music));

            musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            musicViewModel.SelectedAlbum = await dataService.GetAsync<Album, IEnumerable<Track>>(album => album.Id == id, album => album.Tracks);

            return View("Album", musicViewModel);
        }

        public async Task<ActionResult> GetArtist(int id)
        {
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Music));

            musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
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
                Task workItem = null;

                transaction = await transactionService.GetNewTransaction(TransactionTypes.RefreshMusic);
                workItem = fileService.CheckForMusicUpdates(transaction).ContinueWith(task => musicService.ClearData());
                await controllerService.QueueBackgroundWorkItem(ct => workItem, transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }

        public async Task UpdateConfiguration(MusicConfiguration musicConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Music));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Music), JsonData = JsonConvert.SerializeObject(musicConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(musicConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }

        public async Task<JsonResult> GetSong(int id)
        {
            Track track = await dataService.GetAsync<Track, Album, Artist, Genre>(item => item.Id == id,
                                                                                  item => item.Album,
                                                                                  item => item.Artist,
                                                                                  item => item.Genre);
            Song song = new Song
            {
                Id = track.Id,
                Title = track.Title,
                Album = track?.Album.Title,
                Artist = track?.Artist.Name,
                Genre = track?.Genre.Name
            };

            return new JsonResult { Data = song, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task UpdateSong(Song song)
        {
            if (ModelState.IsValid)
            {
                Track track = await dataService.GetAsync<Track, Album, Artist, Genre>(item => item.Id == song.Id,
                                                                                      item => item.Album,
                                                                                      item => item.Artist,
                                                                                      item => item.Genre);
                Album album = await dataService.GetAsync<Album>(item => item.Title == song.Album.Trim());
                Artist artist = await dataService.GetAsync<Artist>(item => item.Name == song.Artist.Trim());
                Genre genre = await dataService.GetAsync<Genre>(item => item.Name == song.Genre.Trim());
                
                if (track != null)
                {
                    if (album == null)
                    {
                        album = new Album(song.Album.Trim());
                        await dataService.Insert(album);
                    }

                    if (artist == null)
                    {
                        artist = new Artist(song.Artist.Trim());
                        await dataService.Insert(artist);
                    }

                    if (genre == null)
                    {
                        genre = new Genre(song.Genre.Trim());
                        await dataService.Insert(genre);
                    }

                    track.Title = song.Title;
                    track.AlbumId = album.Id;
                    track.ArtistId = artist.Id;
                    track.GenreId = genre.Id;
                    await dataService.Update(track);
                    musicService.ClearData();
                }
            }
        }

        public async Task Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "MediaLibrary"),
                       newFile = Path.Combine(path, file.FileName);

                Directory.CreateDirectory(path);
                file.SaveAs(newFile);
                await fileService.ReadMediaFile(newFile, true);
                musicService.ClearData();
            }
        }
    }
}