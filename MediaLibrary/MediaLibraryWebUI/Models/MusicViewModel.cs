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
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class MusicViewModel : ViewModel
    {
        private IEnumerable<IGrouping<string, Track>> songGroups;
        private IEnumerable<IGrouping<string, Artist>> artistGroups;
        private IEnumerable<IGrouping<string, Album>> albumGroups;
        private IEnumerable<Track> songs;
        private IEnumerable<Artist> artists;
        private IEnumerable<Album> albums;
        private readonly HomeViewModel homeViewModel;
        private IEnumerable<Playlist> playlists;
        private MusicConfiguration configuration;
        private Artist selectedArtist;
        private Album selectedAlbum;

        [ImportingConstructor]
        public MusicViewModel(HomeViewModel homeViewModel)
        {
            songGroups = Enumerable.Empty<IGrouping<string, Track>>();
            artistGroups = Enumerable.Empty<IGrouping<string, Artist>>();
            albumGroups = Enumerable.Empty<IGrouping<string, Album>>();
            playlists = Enumerable.Empty<Playlist>();
            songs = Enumerable.Empty<Track>();
            artists = Enumerable.Empty<Artist>();
            albums = Enumerable.Empty<Album>();
            this.homeViewModel = homeViewModel;
            configuration = new MusicConfiguration();
        }

        public IEnumerable<IGrouping<string, Track>> SongGroups { get => songGroups; set => songGroups = value; }
        public IEnumerable<IGrouping<string, Artist>> ArtistGroups { get => artistGroups; set => artistGroups = value; }
        public IEnumerable<IGrouping<string, Album>> AlbumGroups { get => albumGroups; set => albumGroups = value; }
        public SongSort SongSort { get; set; }
        public AlbumSort AlbumSort { get; set; }
        public ArtistSort ArtistSort { get; set; }
        public IEnumerable<Track> Songs { get => songs; set => songs = value; }
        public IEnumerable<Artist> Artists { get => artists; set => artists = value; }
        public IEnumerable<Album> Albums { get => albums; set => albums = value; }
        public HomeViewModel HomeViewModel { get => homeViewModel; }
        public IEnumerable<Playlist> Playlists { get => playlists; set => playlists = value; }
        public MusicConfiguration Configuration { get => configuration; set => configuration = value ?? new MusicConfiguration(); }
        public IEnumerable<SelectListItem> AlbumSortItems { get => MusicRepository.GetAlbumSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() }); }
        public IEnumerable<SelectListItem> ArtistSortItems { get => MusicRepository.GetArtistSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() }); }
        public IEnumerable<SelectListItem> SongSortItems { get => MusicRepository.GetSongSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() }); }
        public Artist SelectedArtist { get => selectedArtist; set => selectedArtist = value; }
        public Album SelectedAlbum { get => selectedAlbum; set => selectedAlbum = value; }
    }
}