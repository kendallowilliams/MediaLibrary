using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using MediaLibraryDAL.Partials.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Podcast: IDataModel, IPodcast
    {
        public Podcast(string title, string url, string imageUrl, string description, string author)
        {
            Title = title;
            Url = url;
            ImageUrl = imageUrl;
            Description = description;
            Author = author;
        }
    }
}
