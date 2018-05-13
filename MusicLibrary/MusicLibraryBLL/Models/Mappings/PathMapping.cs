using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class PathMapping : ClassMapper<TrackPath>
    {
        public PathMapping()
        {
            Table("path");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}