using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models
{
    public class TrackFile : BaseModel
    {
        public TrackFile() { }

        public TrackFile(byte[] data, string type, int trackId)
        {
            Data = data;
            Type = type;
            TrackId = trackId;
        }

        public int TrackId { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}