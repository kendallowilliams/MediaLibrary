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
using static MediaLibraryWebUI.UIEnums;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Models.Data;
using System.Web;
using System.IO;
using IO_File = System.IO.File;
using Fody;

namespace MediaLibraryWebUI.Controllers
{
    [ConfigureAwait(false)]
    [Export(nameof(MediaPages.Music), typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MusicController : BaseController
    {
        private readonly Lazy<IDataService> lazyDataService;
        private readonly Lazy<IMusicUIService> lazyMusicService;
        private readonly Lazy<MusicViewModel> lazyMusicViewModel;
        private readonly Lazy<ITrackService> lazyTrackService;
        private readonly Lazy<IFileService> lazyFileService;
        private readonly Lazy<IControllerService> lazyControllerService;
        private readonly Lazy<ITransactionService> lazyTransactionService;
        private IDataService dataService => lazyDataService.Value;
        private IMusicUIService musicService => lazyMusicService.Value;
        private MusicViewModel musicViewModel => lazyMusicViewModel.Value;
        private ITrackService trackService => lazyTrackService.Value;
        private IFileService fileService => lazyFileService.Value;
        private IControllerService controllerService => lazyControllerService.Value;
        private ITransactionService transactionService => lazyTransactionService.Value;

        [ImportingConstructor]
        public MusicController(Lazy<IDataService> dataService, Lazy<IMusicUIService> musicService, Lazy<MusicViewModel> musicViewModel,
                               Lazy<ITrackService> trackService, Lazy<IFileService> fileService, Lazy<IControllerService> controllerService,
                               Lazy<ITransactionService> transactionService)
        {
            this.lazyDataService = dataService;
            this.lazyMusicService = musicService;
            this.lazyMusicViewModel = musicViewModel;
            this.lazyTrackService = trackService;
            this.lazyFileService = fileService;
            this.lazyControllerService = controllerService;
            this.lazyTransactionService = transactionService;
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Music));

            if (configuration != null)
            {
                musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            }

            await dataService.GetList<Playlist>().ContinueWith(task => musicViewModel.Playlists = task.Result);

            if (musicViewModel.Configuration.SelectedMusicPage == MusicPages.Album &&
                await dataService.Exists<Album>(album => album.Id == musicViewModel.Configuration.SelectedAlbumId))
            {
                result = await GetAlbum(musicViewModel.Configuration.SelectedAlbumId);
            }
            else if (musicViewModel.Configuration.SelectedMusicPage == MusicPages.Artist &&
                     await dataService.Exists<Artist>(artist => artist.Id == musicViewModel.Configuration.SelectedArtistId))
            {
                result = await GetArtist(musicViewModel.Configuration.SelectedArtistId);
            }
            else
            {
                Task songGroupTask = musicService.GetSongGroups(musicViewModel.Configuration.SelectedSongSort).ContinueWith(task => musicViewModel.SongGroups = task.Result),
                     albumGroupTask = musicService.GetAlbumGroups(musicViewModel.Configuration.SelectedAlbumSort).ContinueWith(task => musicViewModel.AlbumGroups = task.Result),
                     artistGroupTask = musicService.GetArtistGroups(musicViewModel.Configuration.SelectedArtistSort).ContinueWith(task => musicViewModel.ArtistGroups = task.Result),
                     songsTask = musicService.Songs().ContinueWith(task => musicViewModel.Songs = task.Result),
                     albumsTask = musicService.Albums().ContinueWith(task => musicViewModel.Albums = task.Result),
                     artistsTask = musicService.Artists().ContinueWith(task => musicViewModel.Artists = task.Result);
                
                await Task.WhenAll(songGroupTask, albumGroupTask, artistGroupTask, songsTask, albumsTask, artistsTask);
                result = PartialView(musicViewModel);
            }

            return result;
        }

        [CompressContent]
        public async Task<ActionResult> GetSongGroup(string key)
        {
            IGrouping<string, Track> group = musicViewModel.SongGroups.FirstOrDefault(item => item.Key == key);
            int playlistCount = await dataService.Count<Playlist>();

            return PartialView("~/Views/Music/SongGroup.cshtml", (Group: group, PlaylistCount: playlistCount));
        }

#if !DEBUG && !DEV
        [AllowAnonymous]
#endif
        public async Task<ActionResult> File(int id)
        {
            Track track = await dataService.Get<Track>(item => item.Id == id, default, item => item.TrackPath);
            ActionResult result = null;

            if (track != null && IO_File.Exists(Path.Combine(track.TrackPath.Location, track.FileName)))
            {
                result = new FileRangeResult(Path.Combine(track.TrackPath.Location, track.FileName),
                                             Request.Headers["Range"], 
                                             MimeMapping.GetMimeMapping(track.FileName));
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
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylistSong);
                await dataService.Insert(item);
                await transactionService.UpdateTransactionCompleted(transaction, $"Playlist: {playlistId}, Track: {itemId}");
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task AddArtistToPlaylist(int itemId, int playlistId)
        {
            Transaction transaction = null;

            try
            {
                IEnumerable<Track> tracks = null;
                IEnumerable<PlaylistTrack> items = null;

                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylistArtist);
                tracks = await dataService.GetList<Track>(track => track.ArtistId == itemId);
                items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });
                await dataService.Insert(items);
                await transactionService.UpdateTransactionCompleted(transaction, $"Playlist: {playlistId}, Artist: {itemId}");
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task AddAlbumToPlaylist(int itemId, int playlistId)
        {
            Transaction transaction = null;

            try
            {
                IEnumerable<Track> tracks = null;
                IEnumerable<PlaylistTrack> items = null;

                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylistAlbum);
                tracks = await dataService.GetList<Track>(track => track.AlbumId == itemId);
                items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });
                await dataService.Insert(items);
                await transactionService.UpdateTransactionCompleted(transaction, $"Playlist: {playlistId}, Album: {itemId}");
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        private async Task<ActionResult> GetAlbum(int id)
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Music));

            musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            musicViewModel.SelectedAlbum = await dataService.Get<Album>(album => album.Id == id, default, album => album.Tracks.Select(track => track.Artist));
            musicViewModel.SelectedAlbum.Tracks = musicViewModel.SelectedAlbum.Tracks?.OrderBy(song => song.Position).ThenBy(song => song.Title).ToList();

            return PartialView("Album", musicViewModel);
        }

        private async Task<ActionResult> GetArtist(int id)
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Music));

            musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            musicViewModel.SelectedArtist = await dataService.Get<Artist>(artist => artist.Id == id, default, artist => artist.Albums.Select(album => album.Tracks));
            return PartialView("Artist", musicViewModel);
        }

        public async Task<ActionResult> Scan(ScanDirectoryRequest request)
        {
            Transaction transaction = null;
            HttpStatusCodeResult result = new HttpStatusCodeResult(HttpStatusCode.Accepted);
            string message = string.Empty;

            try
            {
                Transaction existingTransaction = await transactionService.GetActiveTransactionByType(TransactionTypes.Read);
#if DEBUG
                request = new ScanDirectoryRequest()
                {
                    Path = System.Configuration.ConfigurationManager.AppSettings["MediaLibraryRoot_DEBUG"],
                    Recursive = true
                };
#endif
                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);

                if (request.IsValid())
                {
                    TrackPath path = await dataService.Get<TrackPath>(item => item.Location.Equals(request.Path, StringComparison.CurrentCultureIgnoreCase));

                    if (path != null)
                    {
                        message = $"'{path.Location}' has already been added. Run {nameof(MusicController)} -> {nameof(Refresh)} instead.";
                        result = new HttpStatusCodeResult(HttpStatusCode.Conflict, message);
                        await transactionService.UpdateTransactionCompleted(transaction, message);
                    }
                    else if (existingTransaction == null)
                    {
                        await controllerService.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, request.Path, request.Recursive).ContinueWith(task => musicService.ClearData()),
                                                                              transaction);
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
                await fileService.CheckForMusicUpdates(transaction).ContinueWith(task => musicService.ClearData());
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
                Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Music));

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
            Track track = await dataService.Get<Track>(item => item.Id == id, default, item => item.Album, item => item.Artist, item => item.Genre);
            Song song = new Song
            {
                Id = track.Id,
                Title = track.Title,
                Album = track.Album?.Title,
                Artist = track.Artist?.Name,
                Genre = track.Genre?.Name,
                Position = track.Position
            };

            return new JsonResult { Data = song, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task UpdateSong(Song song)
        {
            if (ModelState.IsValid)
            {
                Track track = await dataService.Get<Track>(item => item.Id == song.Id);
                Album album = await dataService.Get<Album>(item => item.Title == song.Album.Trim());
                Artist artist = await dataService.Get<Artist>(item => item.Name == song.Artist.Trim());
                Genre genre = await dataService.Get<Genre>(item => item.Name == song.Genre.Trim());
                
                if (track != null)
                {
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

                    if (album == null)
                    {
                        album = new Album(song.Album.Trim()) { ArtistId = artist.Id, GenreId = genre.Id };
                        await dataService.Insert(album);
                    }
                    
                    track.Title = song.Title;
                    track.AlbumId = album.Id;
                    track.ArtistId = artist.Id;
                    track.GenreId = genre.Id;
                    track.Position = song.Position;
                    await dataService.Update(track);
                    musicService.ClearData();
                }
            }
        }

        public async Task Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string dtFormat = "yyyyMMddHHmmss",
                       newFile = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString(dtFormat)}{Path.GetExtension(file.FileName)}",
                       filePath = Path.Combine(fileService.MusicFolder, newFile);

                Directory.CreateDirectory(fileService.MusicFolder);
                file.SaveAs(filePath);
                await fileService.ReadMediaFile(filePath);
                musicService.ClearData();
            }
        }

        [CompressContent]
        public async Task<ActionResult> GetAlbums()
        {
            if (musicViewModel.AlbumGroups == null) /*then*/ musicViewModel.AlbumGroups = await musicService.GetAlbumGroups(musicViewModel.AlbumSort);
            musicViewModel.Playlists = await dataService.GetList<Playlist>();

            return PartialView("Albums", musicViewModel);
        }

        [CompressContent]
        public async Task<ActionResult> GetArtists()
        {
            if (musicViewModel.ArtistGroups == null) /*then*/ musicViewModel.ArtistGroups = await musicService.GetArtistGroups(musicViewModel.ArtistSort);
            musicViewModel.Playlists = await dataService.GetList<Playlist>();

            return PartialView("Artists", musicViewModel);
        }

        [CompressContent]
        public async Task<ActionResult> GetSongs()
        {
            if (musicViewModel.SongGroups == null) /*then*/ musicViewModel.SongGroups = await musicService.GetSongGroups(musicViewModel.SongSort);
            musicViewModel.Playlists = await dataService.GetList<Playlist>();

            return PartialView("Songs", musicViewModel);
        }

        public async Task<ActionResult> MusicConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Music));

            if (configuration != null)
            {
                musicViewModel.Configuration = JsonConvert.DeserializeObject<MusicConfiguration>(configuration.JsonData) ?? new MusicConfiguration();
            }

            return Json(musicViewModel.Configuration, JsonRequestBehavior.AllowGet);
        }
    }
}