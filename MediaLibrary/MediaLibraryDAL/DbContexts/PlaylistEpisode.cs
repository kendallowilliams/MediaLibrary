using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class PlaylistEpisode
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int EpisodeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Episode Episode { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}
