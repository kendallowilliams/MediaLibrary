using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI
{
    public static class Enums
    {
        public enum SongSort { AtoZ = 0, Album, Artist, DateAdded }
        public enum ArtistSort { AtoZ = 0 }
        public enum AlbumSort { AtoZ = 0 }
        public enum PodcastSort { AtoZ = 0, DateModified }
        public enum PlaylistSort { AtoZ = 0, DateModified }
        public enum MusicTab { Albums = 0, Artists, Songs }
        public enum MusicPages { Index = 0, Album, Artist }
        public enum PlaylistPages { Index = 0, Playlist }
        public enum PodcastPages { Index = 0, Podcast }
        public enum MediaPages { Home = 0, Music, Playlists, Podcasts }
    }
}