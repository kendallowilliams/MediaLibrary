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
        private IEnumerable<Artist> artists;
        private IEnumerable<Album> albums;
        private SongSort songSort;

        public MusicViewModel()
        {
            songs = Enumerable.Empty<IGrouping<string, Track>>();
            artists = Enumerable.Empty<Artist>();
            albums = Enumerable.Empty<Album>();
        }

        public IEnumerable<IGrouping<string, Track>> Songs { get => songs; set => songs = value; }
        public IEnumerable<Artist> Artists { get => artists; set => artists = value; }
        public IEnumerable<Album> Albums { get => albums; set => albums = value; }
        public SongSort SongSort { get => songSort; set => songSort = value; }
    }
}