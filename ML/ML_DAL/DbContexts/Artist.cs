using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
            Tracks = new HashSet<Track>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
