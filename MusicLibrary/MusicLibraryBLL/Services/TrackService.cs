using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(ITrackService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TrackService : ITrackService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public TrackService()
        { }

        public async Task<int> AddTrack(string title, string fileName, int pathId, int albumId,
                                        int genreId, int artistId, int duration, uint position,
                                        uint year)
        {
            Track track = new Track(title, fileName, pathId, albumId, genreId, artistId, duration, position, year);
            return await InsertTrack(track);
        }

        public async Task<IEnumerable<Track>> GetTracks() => await dataService.GetList<Track>();

        public async Task<Track> GetTrack(object id) => await dataService.Get<Track>(id);

        public async Task<int> InsertTrack(Track track) => await dataService.Insert<Track,int>(track);

        public async Task<bool> DeleteTrack(int id) => await dataService.Delete<Track>(id);

        public async Task<bool> DeleteTrack(Track track) => await dataService.Delete(track);

        public async Task<bool> UpdateTrack(Track track) => await dataService.Update(track);
    }
}