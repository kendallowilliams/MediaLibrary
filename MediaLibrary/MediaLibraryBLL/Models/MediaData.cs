using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Models
{
    public class MediaData
    {
        public MediaData() { }
        
        public string FileName { get; set; }
        public string Artists { get; set; }
        public string Copyright { get; set; }
        public string Title { get; set; }
        public string AlbumArtists { get; set; }
        public string Genres { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public uint Year { get; set; }
        public uint Track { get; set; }
        public uint TrackCount { get; set; }
        public double Duration { get; set; }
    }
}
