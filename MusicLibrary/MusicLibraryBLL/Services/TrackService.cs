using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITrackService))]
    public class TrackService : ITrackService
    {
        private readonly IDataService dataService;
        private readonly string findPathsStoredProcedure = @"FindPaths",
                                deleteAllPathsStoredProcedure = @"DeleteAllPaths",
                                getTrackFileStoredProcedure = @"GetTrackFile";

         [ImportingConstructor]
        public TrackService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IEnumerable<Track>> GetTracks() => await dataService.GetList<Track>();

        public async Task<Track> GetTrack(object id) => await dataService.Get<Track>(id);

        public async Task<int> InsertTrack(Track track) => await dataService.Insert<Track,int>(track);

        public async Task<bool> DeleteTrack(int id) => await dataService.Delete<Track>(id);

        public async Task<bool> DeleteTrack(Track track) => await dataService.Delete(track);

        public async Task DeleteAllTracks() => await dataService.Execute(deleteAllPathsStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdateTrack(Track track) => await dataService.Update(track);

        public async Task<int?> AddPath(string location)
        {

            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(location))
            {
                object parameters = new { location };
                TrackPath path = new TrackPath(location);
                IEnumerable<TrackPath> paths = await dataService.Query<TrackPath>(findPathsStoredProcedure, parameters, CommandType.StoredProcedure);

                if (paths.Any()) { id = paths.FirstOrDefault().Id; }
                else { id = await dataService.Insert<TrackPath, int>(path); }
            }

            return id;
        }

        public async Task<int?> AddTrackFile(int trackId)
        {
            Track track = await GetTrack(trackId);
            TrackPath path = await dataService.Get<TrackPath>(track.PathId);
            TrackFile trackFile = null;
            string filePath = Path.Combine(path.Location, track.FileName);
            byte[] data = File.ReadAllBytes(filePath);

            trackFile = new TrackFile(data, MimeMapping.GetMimeMapping(track.FileName), trackId);
            trackFile.Id = await dataService.Insert<TrackFile, int>(trackFile);

            return trackFile.Id;
        }

        public async Task<TrackFile> GetTrackFile(int id)
        {
            object parameters = new { track_id = id };
            IEnumerable<TrackFile> files = await dataService.Query<TrackFile>(getTrackFileStoredProcedure, parameters, CommandType.StoredProcedure);
            TrackFile file = files.FirstOrDefault();

            if (file == null)
            {
                Track track = await dataService.Get<Track>(id);

                if (track != null)
                {
                    TrackPath path = await dataService.Get<TrackPath>(track.PathId);
                    string fileName = Path.Combine(path.Location, track.FileName);
                    byte[] data = File.ReadAllBytes(fileName);

                    file = new TrackFile { Data = data, Type = MimeMapping.GetMimeMapping(track.FileName) };
                }
            }

            return file;
        }
    }
}