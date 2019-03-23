using MediaLibraryDAL.Models;
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

        Podcast GetPodcast(Expression<Func<Podcast, bool>> expression = null);

        IEnumerable<Podcast> GetPodcasts(Expression<Func<Podcast, bool>> expression = null);

        IEnumerable<PodcastItem> GetPodcastItems(int podcastId);

        Task<int> InsertPodcast(Podcast podcast);

        Task<int> DeletePodcast(int id);

        Task DeleteAllPodcasts();

        Task<int> UpdatePodcast(Podcast podcast);

        Task<Podcast> RefreshPodcast(Podcast podcast);

        Task<int?> AddPodcastFile(Transaction transaction, int podcastItemId);

        PodcastFile GetPodcastFile(int id);
    }
}
