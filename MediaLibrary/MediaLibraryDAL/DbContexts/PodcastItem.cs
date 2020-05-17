using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class PodcastItem
    {
        public PodcastItem()
        {
            PlaylistPodcastItem = new HashSet<PlaylistPodcastItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Length { get; set; }
        public string Url { get; set; }
        public string File { get; set; }
        public int Progress { get; set; }
        public int PodcastId { get; set; }
        public DateTime PublishDate { get; set; }
        public int PlayCount { get; set; }
        public DateTime? LastPlayedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Podcast Podcast { get; set; }
        public virtual ICollection<PlaylistPodcastItem> PlaylistPodcastItem { get; set; }
    }
}
