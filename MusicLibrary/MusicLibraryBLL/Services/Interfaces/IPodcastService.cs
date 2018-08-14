using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<Podcast> AddPodcast(string url);

        Task<Podcast> GetPodcast(object id);

        Task<IEnumerable<Podcast>> GetPodcasts();

        Task<int> InsertPodcast(Podcast podcast);

        Task<int> InsertPodcastItem(PodcastItem podcastItem);

        Task<bool> DeletePodcast(int id);

        Task<bool> DeletePodcast(Podcast podcast);

        Task DeleteAllPodcasts();

        Task<bool> UpdatePodcast(Podcast podcast);

        Task<int?> AddPodcastFile(int podcastItemId);

        Task<PodcastFile> GetPodcastFile(int id);
    }
}
