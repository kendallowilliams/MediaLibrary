using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Artist
    {
        public Artist()
        {
            Album = new HashSet<Album>();
            Track = new HashSet<Track>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Track> Track { get; set; }
    }
}
