using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI
{
    public static class UIEnums
    {
        public enum SongSort { AtoZ = 0, Album, Artist, DateAdded, Genre }
        public enum ArtistSort { AtoZ = 0 }
        public enum AlbumSort { AtoZ = 0 }
        public enum SeriesSort { AtoZ = 0 }
        public enum PodcastSort { LastUpdateDate = 0, AtoZ, DateAdded }
        public enum PodcastFilter { All = 0, Downloaded, Unplayed }
        public enum PlaylistSort { AtoZ = 0, DateAdded }
        public enum MusicTabs { Albums = 0, Artists, Songs }
        public enum MusicPages { Index = 0, Album, Artist }
        public enum PlayerPages { Index = 0, Audio, Video }
        public enum PlaylistPages { Index = 0, Playlist }
        public enum PlaylistTabs { Music = 0, Podcast, Episode }
        public enum PodcastPages { Index = 0, Podcast }
        public enum TelevisionPages { Index = 0, Series }
        public enum MediaPages { Home = 0, Music, Playlist, Podcast, Player, Television }
        public enum RepeatTypes { None = 0, RepeatOne, RepeatAll }
    }
}