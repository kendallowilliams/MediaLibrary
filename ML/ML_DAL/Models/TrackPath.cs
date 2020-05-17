using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models
{
    public class TrackPath : BaseModel
    {
        public TrackPath() { }

        public TrackPath(string location, int? parentPathId = null)
        {
            Location = location;
            ParentPathId = parentPathId;
        }

        public string Location { get; set; }
        public DateTime LastScanDate { get; set; } = DateTime.Now;
        public int? ParentPathId { get; set; }
        public TrackPath ParentPath { get; set; }
    }
}