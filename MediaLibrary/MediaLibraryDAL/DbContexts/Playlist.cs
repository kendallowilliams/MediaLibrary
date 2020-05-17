using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistEpisode = new HashSet<PlaylistEpisode>();
            PlaylistPodcastItem = new HashSet<PlaylistPodcastItem>();
            PlaylistTrack = new HashSet<PlaylistTrack>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<PlaylistEpisode> PlaylistEpisode { get; set; }
        public virtual ICollection<PlaylistPodcastItem> PlaylistPodcastItem { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTrack { get; set; }
    }
}
