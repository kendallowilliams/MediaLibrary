using MediaLibraryDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface ITrackService
    {
        Track GetTrack(Expression<Func<Track, bool>> expression = null);

        IEnumerable<Track> GetTracks(Expression<Func<Track, bool>> expression = null);

        Task<int> InsertTrack(Track track);

        Task<int> DeleteTrack(int id);

        Task<int> DeleteTrack(Track track);

        Task DeleteAllTracks();

        Task<int> UpdateTrack(Track track);

        Task<int?> AddPath(string location);

        Task<int?> AddTrackFile(int trackId);

        TrackFile GetTrackFile(int id);
    }
}
