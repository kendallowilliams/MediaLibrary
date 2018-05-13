using MusicLibraryWebApi.Models;
using MusicLibraryWebApi.Services;
using MusicLibraryWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Podcast/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Podcast/5
        public async Task Delete(int id)
        {
            await podcastService.DeletePodcast(id);
        }
    }
}
