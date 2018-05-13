using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IFileService fileService => MefConfig.Container.GetExportedValue<IFileService>();

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
        public async Task Post([FromBody]string data)
        {
            string path = Path.GetTempFileName();
            await fileService.Write(path, data);
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
