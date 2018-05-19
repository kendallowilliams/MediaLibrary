using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class TrackPathMapping : ClassMapper<TrackPath>
    {
        public TrackPathMapping()
        {
            Table("path");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.LastScanDate).Column("last_scan_date");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}