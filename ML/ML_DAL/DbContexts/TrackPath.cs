using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class TrackPath
    {
        public TrackPath()
        {
            Tracks = new HashSet<Track>();
        }

        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime LastScanDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
