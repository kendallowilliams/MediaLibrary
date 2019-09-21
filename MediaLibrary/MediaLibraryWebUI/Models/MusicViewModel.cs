using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    public class MusicViewModel
    {
        private IEnumerable<IGrouping<string, Track>> songs;
        private IEnumerable<IGrouping<string, Artist>> artists;
        private IEnumerable<IGrouping<string, Album>> albums;

        public MusicViewModel()
        {
            songs = Enumerable.Empty<IGrouping<string, Track>>();
            artists = Enumerable.Empty<IGrouping<string, Artist>>();
            albums = Enumerable.Empty<IGrouping<string, Album>>();
        }

        public IEnumerable<IGrouping<string, Track>> Songs { get => songs; set => songs = value; }
        public IEnumerable<IGrouping<string, Artist>> Artists { get => artists; set => artists = value; }
        public IEnumerable<IGrouping<string, Album>> Albums { get => albums; set => albums = value; }
        public SongSort SongSort { get; set; }
        public AlbumSort AlbumSort { get; set; }
        public ArtistSort ArtistSort { get; set; }
    }
}