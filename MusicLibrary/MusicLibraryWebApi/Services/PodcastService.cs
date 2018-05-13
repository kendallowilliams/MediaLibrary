using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryWebApi.Models;
using MusicLibraryWebApi.Services.Interfaces;

namespace MusicLibraryWebApi.Services
{
    [Export(typeof(IPodcastService))]
    public class PodcastService : IPodcastService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public PodcastService()
        { }

        public async Task<IEnumerable<Podcast>> GetPodcasts() => await dataService.GetList<Podcast>();

        public async Task<Podcast> GetPodcast(object id) =>  await dataService.Get<Podcast>(id);

        public async Task<int> InsertPodcast(Podcast podcast) => await dataService.Insert<Podcast,int>(podcast);

        public async Task<bool> DeletePodcast(int id) => await dataService.Delete<Podcast>(id);

        public async Task<bool> DeletePodcast(Podcast podcast) => await dataService.Delete(podcast);

        public async Task<bool> UpdatePodcast(Podcast podcast) => await dataService.Update(podcast);
    }
}