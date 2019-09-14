using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryBLL.Models
{
    public class PodcastFile : BaseModel
    {
        public PodcastFile() { }

        public PodcastFile(byte[] data, string type, int podcastId, int podcastItemId)
        {
            Data = data;
            Type = type;
            PodcastId = podcastId;
            PodcastItemId = podcastItemId;
        }

        public int PodcastItemId { get; set; }
        public int PodcastId { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}