using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistEpisodes = new HashSet<PlaylistEpisode>();
            PlaylistPodcastItems = new HashSet<PlaylistPodcastItem>();
            PlaylistTracks = new HashSet<PlaylistTrack>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<PlaylistEpisode> PlaylistEpisodes { get; set; }
        public virtual ICollection<PlaylistPodcastItem> PlaylistPodcastItems { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
