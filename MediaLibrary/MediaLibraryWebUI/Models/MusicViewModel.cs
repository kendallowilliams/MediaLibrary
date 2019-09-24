using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class MusicViewModel
    {
        private IEnumerable<IGrouping<string, Track>> songGroups;
        private IEnumerable<IGrouping<string, Artist>> artistGroups;
        private IEnumerable<IGrouping<string, Album>> albumGroups;
        private IEnumerable<Track> songs;
        private IEnumerable<Artist> artists;
        private IEnumerable<Album> albums;
        private readonly HomeViewModel homeViewModel;
        private IEnumerable<Playlist> playlists;

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
    }
}