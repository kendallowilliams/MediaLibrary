using MusicLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface ITrackService
    {
        Task<int> AddTrack(string title, string fileName, int pathId, int albumId,
                           int genreId, int artistId, int duration, uint track,
                           uint year);

        Task<Track> GetTrack(object id);

        Task<IEnumerable<Track>> GetTracks();

        Task<int> InsertTrack(Track track);

        Task<bool> DeleteTrack(int id);

        Task<bool> DeleteTrack(Track track);

        Task<bool> UpdateTrack(Track track);
    }
}
