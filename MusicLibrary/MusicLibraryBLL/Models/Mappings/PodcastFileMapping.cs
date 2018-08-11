using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class PodcastFileMapping : ClassMapper<PodcastFile>
    {
        public PodcastFileMapping()
        {
            Table("podcast_file");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}