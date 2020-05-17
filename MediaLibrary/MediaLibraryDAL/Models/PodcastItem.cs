using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models
{
    public class PodcastItem : BaseModel
    {
        public PodcastItem() { }

        public PodcastItem(string title, string description, string url, long length, DateTime publishDate, int podcastId)
        {
            Title = title;
            Url = url;
            Description = description;
            Length = length;
            PublishDate = publishDate;
            PodcastId = podcastId;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long Length { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.MinValue;
        public int PodcastId { get; set; }
        public PodcastFile File { get; set; }
    }
}