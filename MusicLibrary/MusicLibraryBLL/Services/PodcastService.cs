using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPodcastService))]
    public class PodcastService : IPodcastService
    {
        private readonly IDataService dataService;
        private readonly IWebService webService;

        [ImportingConstructor]
        public PodcastService(IDataService dataService, IWebService webService)
        {
            this.dataService = dataService;
            this.webService = webService;
        }

        public async Task AddPodcast(Podcast podcast)
        {
            byte[] data = await webService.DownloadData(podcast.Url);
        }

        public async Task<IEnumerable<Podcast>> GetPodcasts() => await dataService.GetList<Podcast>();

        public async Task<Podcast> GetPodcast(object id) =>  await dataService.Get<Podcast>(id);

        public async Task<int> InsertPodcast(Podcast podcast) => await dataService.Insert<Podcast,int>(podcast);

        public async Task<bool> DeletePodcast(int id) => await dataService.Delete<Podcast>(id);

        public async Task<bool> DeletePodcast(Podcast podcast) => await dataService.Delete(podcast);

        public async Task DeleteAllPodcasts() => await dataService.Execute(@"DELETE podcast;");

        public async Task<bool> UpdatePodcast(Podcast podcast) => await dataService.Update(podcast);
    }
}