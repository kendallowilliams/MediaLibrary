using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class TrackMapping : ClassMapper<Track>
    {
        public TrackMapping()
        {
            Table("track");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.FileName).Column("file_name");
            Map(x => x.PathId).Column("path_id");
            Map(x => x.FileId).Column("file_id");
            Map(x => x.AlbumId).Column("album_id");
            Map(x => x.GenreId).Column("genre_id");
            Map(x => x.ArtistId).Column("artist_id");
            Map(x => x.PlayCount).Column("play_count");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            Map(x => x.DurationDisplay).Ignore();

            AutoMap();
        }
    }
}