using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MusicLibraryWebApi.Controllers
{
    public class PodcastController : ApiController
    {
        private IPodcastService podcastService => MefConfig.Container.GetExportedValue<IPodcastService>();

        // GET: api/Podcast
        public async Task<IEnumerable<Podcast>> Get()
        {
            return await podcastService.GetPodcasts();
        }

        // GET: api/Podcast/5
        public async Task<Podcast> Get(int id)
        {
            return await podcastService.GetPodcast(id);
        }

        // POST: api/Podcast
        public async Task<int> Post([FromBody] Podcast podcast)
        {
            return await podcastService.InsertPodcast(podcast);
        }

        // PUT: api/Podcast/5
        public async Task<bool> Put([FromBody] Podcast podcast)
        {
            return await podcastService.UpdatePodcast(podcast);
        }

        // DELETE: api/Podcast/5
        public async Task<bool> Delete(int id)
        {
            return await podcastService.DeletePodcast(id);
        }
    }
}
