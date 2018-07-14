using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class TrackFile : BaseModel
    {
        public TrackFile() { }

        public TrackFile(int trackId, byte[] data, string name)
        {
            TrackId = trackId;
            Data = data;
            Name = name;
        }

        public int TrackId { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}