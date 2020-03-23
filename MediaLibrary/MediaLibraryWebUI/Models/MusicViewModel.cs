using MediaLibraryBLL.Models.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Repositories;
using MediaLibraryWebUI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class MusicViewModel : ViewModel<MusicConfiguration>
    {
        [ImportingConstructor]
        public MusicViewModel()
        {
            SongGroups = Enumerable.Empty<IGrouping<string, Track>>();
            ArtistGroups = Enumerable.Empty<IGrouping<string, Artist>>();
            AlbumGroups = Enumerable.Empty<IGrouping<string, Album>>();
            Playlists = Enumerable.Empty<Playlist>();
            Songs = Enumerable.Empty<Track>();
            Artists = Enumerable.Empty<Artist>();
            Albums = Enumerable.Empty<Album>();
            AlbumSortItems = MusicRepository.GetAlbumSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
            ArtistSortItems = MusicRepository.GetArtistSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
            SongSortItems = MusicRepository.GetSongSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
        }

        public IEnumerable<IGrouping<string, Track>> SongGroups { get; set; }
        public IEnumerable<IGrouping<string, Artist>> ArtistGroups { get; set; }
        public IEnumerable<IGrouping<string, Album>> AlbumGroups { get; set; }
        public SongSort SongSort { get; set; }
        public AlbumSort AlbumSort { get; set; }
        public ArtistSort ArtistSort { get; set; }
        public IEnumerable<Track> Songs { get; set; }
        public IEnumerable<Artist> Artists { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
        public IEnumerable<SelectListItem> AlbumSortItems { get; }
        public IEnumerable<SelectListItem> ArtistSortItems { get; }
        public IEnumerable<SelectListItem> SongSortItems { get; }
        public Artist SelectedArtist { get; set; }
        public Album SelectedAlbum { get; set; }
    }
}