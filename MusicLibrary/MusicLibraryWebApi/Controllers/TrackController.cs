using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MusicLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TrackController : ApiController
    {
        private ITrackService trackService;
        private IFileService fileService;

        [ImportingConstructor]
        public TrackController(ITrackService trackService, IFileService fileService)
        {
            this.trackService = trackService;
            this.fileService = fileService;
        }

        // GET: api/Track
        public async Task<IEnumerable<Track>> Get()
        {
            return (await trackService.GetTracks()).OrderBy(track => track.Title);
        }

        // GET: api/Track/5
        public async Task<Track> Get(int id)
        {
            return await trackService.GetTrack(id);
        }

        // POST: api/Track
        public async Task<HttpResponseMessage> Post()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                if (files.AllKeys.Any())
                {
                    foreach (string key in files.AllKeys)
                    {
                        string directory = Path.GetTempPath(),
                               path = Path.Combine(directory, files[key].FileName);
                        await Task.Run(() => files[key].SaveAs(path));
                        await fileService.ReadMediaFile(path, true);
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        // PUT: api/Track/5
        public void Put(int id, [FromBody]Track track)
        {

        }

        // DELETE: api/Track/5
        public async Task Delete(int id)
        {
            await trackService.DeleteTrack(id);
        }
    }
}
