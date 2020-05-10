using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryDAL
{
    public static class Enums
    {
        public enum PlaylistTypes { Music, Podcast, Television }
        public enum TransactionTypes : int
        {
            /* Tracks: 0 - 50 */
            GetTracks = 0, GetTrack = 1, AddTrack = 2, RemoveTrack = 3, ReplaceTrack = 4, GetTrackFile = 5,

            /* Albums: 51 - 100 */
            GetAlbums = 51, GetAlbum = 52, AddAlbum = 53, RemoveAlbum = 54, ReplaceAlbum = 55,

            /* Artists: 101 - 150 */
            GetArtists = 101, GetArtist = 102, AddArtist = 103, RemoveArtist = 104, ReplaceArtist = 105,

            /* Genres: 151 - 200 */
            GetGenres = 151, GetGenre = 152,

            /* Playlists: 201 - 250 */
            GetPlaylists = 201, GetPlaylist = 202, AddPlaylist = 203, RemovePlaylist = 204, ReplacePlaylist = 205, AddPlaylistSong = 206, AddPlaylistArtist = 207,
            AddPlaylistAlbum = 208, AddPlaylistPodcastItem = 209, AddPlaylistEpisode = 210,

            /* Podcasts: 251 - 300 */
            GetPodcasts = 251, GetPodcast = 252, AddPodcast = 253, RemovePodcast = 254, ReplacePodcast = 255, DownloadEpisode = 256, DownloadAllEpisodes = 257,
            GetPodcastItems = 258, RefreshPodcast = 259, GetPodcastFile = 260,

            /* Music: 301 - 350 */
            Read = 301, ResetData = 302, RefreshMusic = 303,

            /* Audit Logging */
            LogInfo = 901, LogWarning = 902, LogError = 903,

            None = -1
        }

        public enum TransactionStatus : int
        {
            NotStarted,
            Started,
            InProcess,
            Errored,
            Cancelled,
            Completed
        }
    }
}
