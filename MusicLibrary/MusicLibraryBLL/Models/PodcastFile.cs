using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class PodcastFile : BaseModel
    {
        public PodcastFile() { }

        public PodcastFile(byte[] data, string type)
        {
            Data = data;
            Type = type;
        }

        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}