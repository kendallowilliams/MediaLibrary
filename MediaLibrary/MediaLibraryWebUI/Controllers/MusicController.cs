using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.ActionResults;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.Enums;
using MediaLibraryWebUI.Attributes;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Music", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MusicController : Controller
    {
        private readonly IDataService dataService;
        private readonly IMusicUIService musicService;
        private readonly MusicViewModel musicViewModel;

        [ImportingConstructor]
        public MusicController(IDataService dataService, IMusicUIService musicService, MusicViewModel musicViewModel)
        {
            this.dataService = dataService;
            this.musicService = musicService;
            this.musicViewModel = musicViewModel;
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

        public ActionResult File(int id)
        {
            TrackFile file = dataService.Get<TrackFile>(item => item.Id == id);
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

        public async Task<ActionResult> AddTrackToPlaylist(int itemId, int playlistId)
        {
            PlaylistTrack item = new PlaylistTrack() { PlaylistId = playlistId, TrackId = itemId };

            await dataService.Insert(item);

            return await Index();
        }

        public async Task<ActionResult> AddArtistToPlaylist(int itemId, int playlistId)
        {
            IEnumerable<Track> tracks = await dataService.GetList<Track>(track => track.ArtistId == itemId);
            IEnumerable<PlaylistTrack> items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });

            await dataService.Insert(items);

            return await Index();
        }

        public async Task<ActionResult> AddAlbumToPlaylist(int itemId, int playlistId)
        {
            IEnumerable<Track> tracks = await dataService.GetList<Track>(track => track.AlbumId == itemId);
            IEnumerable<PlaylistTrack> items = tracks.Select(track => new PlaylistTrack { TrackId = track.Id, PlaylistId = playlistId });

            await dataService.Insert(items);

            return await Index();
        }
    }
}