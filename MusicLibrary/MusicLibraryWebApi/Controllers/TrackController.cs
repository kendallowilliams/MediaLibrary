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
    public class TrackController : ApiController
    {
        private ITrackService trackService => MefConfig.Container.GetExportedValue<ITrackService>();

        // GET: api/Track
        public async Task<IEnumerable<Track>> Get()
        {
            return await trackService.GetTracks();
        }

        // GET: api/Track/5
        public async Task<Track> Get(int id)
        {
            return await trackService.GetTrack(id);
        }

        // POST: api/Track
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Track/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Track/5
        public async Task Delete(int id)
        {
            await trackService.DeleteTrack(id);
        }
    }
}
