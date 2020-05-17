using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<Podcast> AddPodcast(string url);

        Task<Podcast> RefreshPodcast(Podcast podcast);

        Task<string> AddPodcastFile(Transaction transaction, int podcastItemId);
    }
}
