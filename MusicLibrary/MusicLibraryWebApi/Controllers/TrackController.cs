using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
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
        private IArtistService artistService => MefConfig.Container.GetExportedValue<IArtistService>();
        private IAlbumService albumService => MefConfig.Container.GetExportedValue<IAlbumService>();
        private IGenreService genreService => MefConfig.Container.GetExportedValue<IGenreService>();
        private IFileService fileService => MefConfig.Container.GetExportedValue<IFileService>();
        private IId3Service id3Service => MefConfig.Container.GetExportedValue<IId3Service>();

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
                    MediaData data = await id3Service.ProcessFile(path);
                    int? genreId = await genreService.AddGenre(data.Genres),
                        artistId = await artistService.AddArtist(data.Artists),
                        albumId = await albumService.AddAlbum(new Album(data, artistId, genreId)),
                        pathId = await trackService.AddPath(directory);
                    Track track = new Track(data, pathId, genreId, albumId, artistId);
                    await trackService.InsertTrack(track);
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
    }
}
