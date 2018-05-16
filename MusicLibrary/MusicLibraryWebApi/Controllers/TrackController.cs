using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        public async Task<HttpResponseMessage> Post()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                foreach (string key in files.AllKeys)
                {
                    string directory = Path.GetTempPath(),
                           path = Path.Combine(directory, files[key].FileName);
                    await Task.Run(() => files[key].SaveAs(path));
                    await fileService.ReadMediaFile(path, true);
                }
            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
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

        public async Task<HttpResponseMessage> Read()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string data = HttpContext.Current.Request.Params["data"];
            IDictionary<string,string> dictionary = data?.Split(new[] { '&' })
                                                         .Where(pair => pair.Contains("="))
                                                         .Select(pair => pair.Split(new[] { '=' }))
                                                         .ToDictionary(pair => pair[0], pair => pair[1]);
            string path = string.Empty;
            bool copyFiles = false,
                 recursive = false;
            bool validData = !string.IsNullOrWhiteSpace(data) &&
                             dictionary.TryGetValue("path", out path) &&
                             dictionary.TryGetValue("copy", out string inCopyFiles) && 
                             bool.TryParse(inCopyFiles, out copyFiles) &&
                             dictionary.TryGetValue("recursive", out string inRecursive) && 
                             bool.TryParse(inRecursive, out recursive);
            try
            {
                TimeSpan begin = DateTime.Now.TimeOfDay,
                         end = DateTime.MaxValue.TimeOfDay;
                if (validData) { await fileService.ReadDirectory(path, recursive, copyFiles); }
                else response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Invalid Data: {JsonConvert.SerializeObject(dictionary)}");
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
