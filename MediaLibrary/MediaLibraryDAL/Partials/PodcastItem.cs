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
    public partial class PodcastItem: BaseModel
    {
        public PodcastItem(string title, string description, string url, int length, DateTime publishDate, int podcastId)
        {
            Title = title;
            Url = url;
            Description = description;
            Length = length;
            PublishDate = publishDate;
            PodcastId = podcastId;
        }
    }
}
