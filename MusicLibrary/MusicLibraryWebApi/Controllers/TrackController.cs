using MediaLibraryDAL.Models;
using MediaLibraryBLL.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryWebApi.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TrackController : ApiControllerBase
    {
        private readonly ITrackService trackService;
        private readonly IFileService fileService;

        [ImportingConstructor]
        public TrackController(ITrackService trackService, IFileService fileService, ITransactionService transactionService)
        {
            this.trackService = trackService;
            this.fileService = fileService;
            this.transactionService = transactionService;
        }

        // GET: api/Track
        public async Task<IEnumerable<Track>> Get()
        {
            IEnumerable<Track> tracks = Enumerable.Empty<Track>();
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetTracks);
                tracks = trackService.GetTracks();
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return tracks.OrderBy(track => track.Title);
        }

        // GET: api/Track/5
        public async Task<Track> Get(int id)
        {
            Transaction transaction = null;
            Track track = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetTrack);
                track = trackService.GetTrack(item => item.Id == id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return track;
        }

        // POST: api/Track
        public async Task<HttpResponseMessage> Post()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddTrack);

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

                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
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
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.RemoveTrack);
                await trackService.DeleteTrack(id);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        [Route("api/Track/GetTrackFile/{id}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTrackFile(int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            Transaction transaction = null;
            string range = HttpContext.Current.Request.Headers["Range"];

            try
            {
                TrackFile file = null;
                MemoryStream stream = null;

                transaction = await transactionService.GetNewTransaction(TransactionTypes.GetTrackFile);
                file = trackService.GetTrackFile(id);

                if (file == null) { throw new FileNotFoundException(); }
                stream = new MemoryStream(file?.Data);

                if (Request.Headers.Range == null)
                {
                    message.Content = new StreamContent(stream);
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.Type);
                }
                else
                {
                    message.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, file.Type);
                }
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(FileNotFoundException)) { message.StatusCode = HttpStatusCode.NotFound; }
                message.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(ex.Message)));
                message.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return message;
        }
    }
}
