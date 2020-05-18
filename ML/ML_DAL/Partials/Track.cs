using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using MediaLibraryDAL.Partials.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Track: IPlayableItem, ITrackJSON
    {
        public Track(MediaData data, int? pathId, int? genreId, int? albumId, int? artistId) : base()
        {
            Title = data.Title;
            FileName = data.FileName;
            PathId = pathId;
            AlbumId = albumId;
            GenreId = genreId;
            ArtistId = artistId;
            Position = (int)data.Track;
            Year = (int)data.Year;
            Duration = (decimal)data.Duration;
            PlayCount = 0;
        }
    }
}
