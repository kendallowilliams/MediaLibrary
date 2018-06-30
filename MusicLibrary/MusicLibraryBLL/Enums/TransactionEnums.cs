using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Enums
{
    public static class TransactionEnums
    {
        public enum TransactionTypes
        {
            GetTracks, GetTrack, AddTrack, RemoveTrack, ReplaceTrack,

            GetAlbums, GetAlbum, AddAlbum, RemoveAlbum, ReplaceAlbum,

            GetArtists, GetArtist, AddArtist, RemoveArtist, ReplaceArtist,

            GetGenres, GetGenre,

            GetPlaylists, GetPlaylist, AddPlaylist, RemovePlaylist, ReplacePlaylist,

            GetPodcasts, GetPodcast, AddPodcast, RemovePodcast, ReplacePodcast,

            Read,
            None = -1
        }

        public enum TransactionStatus
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
