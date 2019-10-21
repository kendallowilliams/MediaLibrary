using MediaLibraryBLL.Models;
using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Repositories
{
    public static class MusicRepository
    {
        public static IEnumerable<IListItem<object, AlbumSort>> GetAlbumSortItems()
        {
            yield return new ListItem<object, AlbumSort>(null, nameof(AlbumSort.AtoZ), AlbumSort.AtoZ);
        }

        public static IEnumerable<IListItem<object, ArtistSort>> GetArtistSortItems()
        {
            yield return new ListItem<object, ArtistSort>(null, nameof(ArtistSort.AtoZ), ArtistSort.AtoZ);
        }

        public static IEnumerable<IListItem<object, SongSort>> GetSongSortItems()
        {
            yield return new ListItem<object, SongSort>(null, "Date added", SongSort.DateAdded);
            yield return new ListItem<object, SongSort>(null, nameof(SongSort.AtoZ), SongSort.AtoZ);
            yield return new ListItem<object, SongSort>(null, nameof(SongSort.Album), SongSort.Album);
            yield return new ListItem<object, SongSort>(null, nameof(SongSort.Artist), SongSort.Artist);
        }
    }
}