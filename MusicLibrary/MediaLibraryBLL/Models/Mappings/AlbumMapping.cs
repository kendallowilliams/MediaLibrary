using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MediaLibraryBLL.Models.Mappings
{
    public class AlbumMapping : ClassMapper<Album>
    {
        public AlbumMapping()
        {
            Table("album");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ArtistId).Column("artist_id");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");
            Map(x => x.GenreId).Column("genre_id");

            AutoMap();
        }
    }
}