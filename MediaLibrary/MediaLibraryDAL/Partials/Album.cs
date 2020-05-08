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
    public partial class Album: IDataModel
    {
        public Album(string title): base()
        {
            Id = default(int);
            Title = title;
            ArtistId = default(int);
            GenreId = default(int);
        }

        public Album(int id, string title): base()
        {
            Id = id;
            Title = title;
            ArtistId = default(int);
            GenreId = default(int);
        }

        public Album(MediaData data, int? artistId, int? genreId): base()
        {
            Title = data.Album;
            ArtistId = artistId;
            GenreId = genreId;
            Year = (int)data.Year;
        }
    }
}
