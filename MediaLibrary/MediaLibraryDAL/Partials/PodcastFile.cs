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
    public partial class PodcastFile: BaseModel
    {
        public PodcastFile(byte[] data, string type, int podcastId, int podcastItemId)
        {
            Data = data;
            Type = type;
            PodcastId = podcastId;
            PodcastItemId = podcastItemId;
        }
    }
}
