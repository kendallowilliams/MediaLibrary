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

        public async Task<HttpResponseMessage> Read([FromBody] JObject inData)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);

            try
            {
                string path = inData["path"]?.ToString();
                bool copyFiles = false,
                     recursive = false,
                     validData = !string.IsNullOrWhiteSpace(path) &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles) &&
                                 bool.TryParse(inData["recursive"]?.ToString(), out recursive);
                TimeSpan begin = DateTime.Now.TimeOfDay,
                         end = DateTime.MaxValue.TimeOfDay;
                if (validData) { await fileService.ReadDirectory(path, recursive, copyFiles); }
                else response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid Data: [{inData}]");
                end = DateTime.Now.TimeOfDay;
                System.Diagnostics.Debug.WriteLine($"Total Time: {end - begin}");
            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
