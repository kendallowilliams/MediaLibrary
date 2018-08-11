﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class PodcastItemMapping : ClassMapper<PodcastItem>
    {
        public PodcastItemMapping()
        {
            Table("podcast_item");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.FileId).Column("file_id");
            Map(x => x.PodcastId).Column("podcast_id");
            Map(x => x.PublishDate).Column("publish_date");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}