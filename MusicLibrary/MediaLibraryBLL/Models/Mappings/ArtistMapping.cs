using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MediaLibraryBLL.Models.Mappings
{
    public class ArtistMapping : ClassMapper<Artist>
    {
        public ArtistMapping()
        {
            Table("artist");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}