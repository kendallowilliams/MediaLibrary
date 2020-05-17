using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Album
    {
        public Album()
        {
            Tracks = new HashSet<Track>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? ArtistId { get; set; }
        public int? Year { get; set; }
        public int? GenreId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
