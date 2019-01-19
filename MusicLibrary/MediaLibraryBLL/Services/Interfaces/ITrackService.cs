using MediaLibraryBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface ITrackService
    {
        Task<Track> GetTrack(object id);

        Task<IEnumerable<Track>> GetTracks();

        Task<int> InsertTrack(Track track);

        Task<bool> DeleteTrack(int id);

        Task<bool> DeleteTrack(Track track);

        Task DeleteAllTracks();

        Task<bool> UpdateTrack(Track track);

        Task<int?> AddPath(string location);

        Task<int?> AddTrackFile(int trackId);

        Task<TrackFile> GetTrackFile(int id);
    }
}
