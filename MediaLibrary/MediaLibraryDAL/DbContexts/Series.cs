using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Series
    {
        public Series()
        {
            Episodes = new HashSet<Episode>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
