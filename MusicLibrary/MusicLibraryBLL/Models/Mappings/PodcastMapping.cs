using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class PodcastMapping : ClassMapper<Podcast>
    {
        public PodcastMapping()
        {
            Table("podcast");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ImageUrl).Column("image_url");
            Map(x => x.LastUpdateDate).Column("last_update_date");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}