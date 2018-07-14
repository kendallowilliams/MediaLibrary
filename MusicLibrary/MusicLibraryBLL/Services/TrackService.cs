using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public TrackService()
        { }

        public async Task<IEnumerable<Track>> GetTracks() => await dataService.GetList<Track>();

        public async Task<Track> GetTrack(object id) => await dataService.Get<Track>(id);

        public async Task<int> InsertTrack(Track track) => await dataService.Insert<Track,int>(track);

        public async Task<bool> DeleteTrack(int id) => await dataService.Delete<Track>(id);

        public async Task<bool> DeleteTrack(Track track) => await dataService.Delete(track);

        public async Task<bool> UpdateTrack(Track track) => await dataService.Update(track);

        public async Task<int?> AddPath(string location)
        {

            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(location))
            {
                string existsQuery = $"SELECT id FROM path WHERE location = @location";
                TrackPath path = new TrackPath(location);

                id = await dataService.ExecuteScalar<int?>(existsQuery, new { location });

                if (!id.HasValue)
                {
                    id = await dataService.Insert<TrackPath, int>(path);
                }
            }

            return id;
        }

        public async Task<int?> AddTrackFile(int trackId, int pathId, string fileName)
        {
            TrackPath path = await dataService.Get<TrackPath>(pathId);
            string filePath = Path.Combine(path.Location, fileName);
            byte[] data = File.ReadAllBytes(fileName);
            TrackFile file = new TrackFile(trackId, data, fileName);

            return await dataService.Insert<TrackFile,int>(file);
        }

        public async Task<TrackFile> GetTrackFile(int id)
        {
            Track track = await GetTrack(id);
            TrackFile file = null;

            if (track.FileId.HasValue)
            {
                file = await dataService.Get<TrackFile>(id);
            }
            else
            {
                TrackPath path = await dataService.Get<TrackPath>(track.PathId);
                string fileName = Path.Combine(path.Location, track.FileName);
                byte[] data = File.ReadAllBytes(fileName);

                file = new TrackFile { Name = fileName, Data = data };
            }

            return file;
        }
    }
}