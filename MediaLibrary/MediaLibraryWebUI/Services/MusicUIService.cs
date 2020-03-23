using Fody;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IMusicUIService))]
    public class MusicUIService : BaseUIService, IMusicUIService
    {
        private readonly IDataService dataService;
        private IEnumerable<Track> songs;
        private IEnumerable<Artist> artists;
        private IEnumerable<Album> albums;

        [ImportingConstructor]
        public MusicUIService(IDataService dataService) : base()
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Track>> Songs() => songs ?? await dataService.GetList<Track>();
        public async Task<IEnumerable<Artist>> Artists() => artists ?? await dataService.GetList<Artist>();
        public async Task<IEnumerable<Album>> Albums() => albums ?? await dataService.GetList<Album>();

        public async Task<IEnumerable<IGrouping<string, Track>>> GetSongGroups(SongSort sort)
        {
            IEnumerable<IGrouping<string, Track>> groups = null;

            if (songs == null) /*then*/ songs = (await dataService.GetList<Track>(default, default, 
                song => song.Album, 
                song => song.Artist,
                song => song.Genre))?.OrderBy(song => song.Title);

            switch(sort)
            {
                case SongSort.Album:
                    groups = songs.GroupBy(song => song.Album.Title).OrderBy(group => group.Key);
                    break;
                case SongSort.Artist:
                    groups = songs.GroupBy(song => song.Artist?.Name ?? "Unknown Artist").OrderBy(group => group.Key);
                    break;
                case SongSort.DateAdded:
                    groups = songs.GroupBy(song => song.CreateDate.ToString("yyyy-MM-dd")).OrderByDescending(group => DateTime.Parse(group.Key));
                    break;
                case SongSort.Genre:
                    groups = songs.GroupBy(song => song.Genre?.Name ?? "Unknown Genre").OrderBy(group => group.Key);
                    break;
                case SongSort.AtoZ:
                default:
                    groups = GetSongsAtoZ(songs);
                    break;
            }

            return groups;
        }

        public async Task<IEnumerable<IGrouping<string, Album>>> GetAlbumGroups(AlbumSort sort)
        {
            IEnumerable<IGrouping<string, Album>> groups = null;

            if (albums == null) /*then*/ albums = (await dataService.GetList<Album>(default, default, album => album.Artist, album => album.Tracks))
                                                         .Where(album => album.Tracks.Any());

            switch (sort)
            {
                case AlbumSort.AtoZ:
                    groups = GetAlbumsAtoZ(albums.OrderBy(album => album.Title));
                    break;
            }

            return groups;
        }

        public async Task<IEnumerable<IGrouping<string, Artist>>> GetArtistGroups(ArtistSort sort)
        {
            IEnumerable<IGrouping<string, Artist>> groups = null;

            if (artists == null) /*then*/ artists = (await dataService.GetList<Artist>(default, default, artist => artist.Albums))
                                                           .Where(artist => artist.Albums.Any());

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
            return songs.GroupBy(track => getCharLabel(track.Title)).OrderBy(group => group.Key);
        }

        private IEnumerable<IGrouping<string, Album>> GetAlbumsAtoZ(IEnumerable<Album> albums)
        {
            return albums.GroupBy(album => getCharLabel(album.Title)).OrderBy(group => group.Key);
        }

        private IEnumerable<IGrouping<string, Artist>> GetArtistsAtoZ(IEnumerable<Artist> artists)
        {
            return artists.GroupBy(artist => getCharLabel(artist.Name)).OrderBy(group => group.Key);
        }

        public void ClearData()
        {
            songs = null;
            artists = null;
            albums = null;
        }
    }
}