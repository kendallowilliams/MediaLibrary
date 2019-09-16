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

        Task<Podcast> GetPodcast(Expression<Func<Podcast, bool>> expression = null);

        Task<IEnumerable<Podcast>> GetPodcasts(Expression<Func<Podcast, bool>> expression = null);

        Task<IEnumerable<PodcastItem>> GetPodcastItems(int podcastId);

        Task<int> InsertPodcast(Podcast podcast);

        Task<int> DeletePodcast(int id);

        Task DeleteAllPodcasts();

        Task<int> UpdatePodcast(Podcast podcast);

        Task<Podcast> RefreshPodcast(Podcast podcast);

        Task<int?> AddPodcastFile(Transaction transaction, int podcastItemId);

        Task<PodcastFile> GetPodcastFile(int id);
    }
}
