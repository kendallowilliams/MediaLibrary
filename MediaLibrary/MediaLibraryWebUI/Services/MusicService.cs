using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Services
{
    [Export(typeof(IMusicService))]
    public class MusicService : IMusicService
    {
        private Func<string, string> getLabel;

        [ImportingConstructor]
        public MusicService()
        {
            getLabel = title =>
            {
                char first = title.ToUpper().First();
                string label = string.Empty;

                if (Char.IsLetter(first)) { label = first.ToString(); }
                else if (Char.IsDigit(first)) { label = "#"; }
                else label = "&";

                return label;
            };
        }

        public IEnumerable<IGrouping<string, Track>> GetSongGroups(IEnumerable<Track> songs, SongSort sort)
        {
            IEnumerable < IGrouping<string, Track>> groups = null;

            switch(sort)
            {
                case SongSort.AtoZ:
                    groups = GetSongsAtoZ(songs.OrderBy(song => song.Title));
                    break;
            }

            return groups;
        }

        public IEnumerable<IGrouping<string, Album>> GetAlbumGroups(IEnumerable<Album> albums, AlbumSort sort)
        {
            IEnumerable<IGrouping<string, Album>> groups = null;

            switch (sort)
            {
                case AlbumSort.AtoZ:
                    groups = GetAlbumsAtoZ(albums.OrderBy(album => album.Title));
                    break;
            }

            return groups;
        }
        public IEnumerable<IGrouping<string, Artist>> GetArtistGroups(IEnumerable<Artist> artists, ArtistSort sort)
        {
            IEnumerable<IGrouping<string, Artist>> groups = null;

            switch (sort)
            {
                case ArtistSort.AtoZ:
                    groups = GetArtistsAtoZ(artists.OrderBy(artist => artist.Name));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Track>> GetSongsAtoZ(IEnumerable<Track> songs)
        {
            return songs.GroupBy(track => getLabel(track.Title)).OrderBy(group => group.Key);
        }

        private IEnumerable<IGrouping<string, Album>> GetAlbumsAtoZ(IEnumerable<Album> albums)
        {
            return albums.GroupBy(album => getLabel(album.Title)).OrderBy(group => group.Key);
        }

        private IEnumerable<IGrouping<string, Artist>> GetArtistsAtoZ(IEnumerable<Artist> artists)
        {
            return artists.GroupBy(artist => getLabel(artist.Name)).OrderBy(group => group.Key);
        }
    }
}