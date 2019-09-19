using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Album: BaseModel
    {
        public Album(int id, string title)
        {
            Id = id;
            Title = title;
            ArtistId = -1;
            GenreId = -1;
        }

        public Album(MediaData data, int? artistId, int? genreId)
        {
            Title = data.Album;
            ArtistId = artistId;
            GenreId = genreId;
            Year = (int)data.Year;
        }
    }
}
