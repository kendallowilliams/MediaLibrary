using MusicLibraryBLL.Services.Interfaces;
using Newtonsoft.Json.Linq;
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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class RootController : ApiController
    {
        private readonly IFileService fileService;

        [ImportingConstructor]
        public RootController(IFileService fileService)
        {
            this.fileService = fileService;
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
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
