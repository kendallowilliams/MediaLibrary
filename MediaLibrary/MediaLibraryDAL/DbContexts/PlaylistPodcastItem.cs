using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class PlaylistPodcastItem
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int PodcastItemId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual PodcastItem PodcastItem { get; set; }
    }
}
