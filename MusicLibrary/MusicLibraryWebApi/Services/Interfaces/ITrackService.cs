using MusicLibraryWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryWebApi.Services.Interfaces
{
    interface ITrackService
    {
        Task<Track> GetTrack(object id);

        Task<IEnumerable<Track>> GetTracks();

        Task<int> InsertTrack(Track track);

        Task<bool> DeleteTrack(int id);

        Task<bool> DeleteTrack(Track track);

        Task<bool> UpdateTrack(Track track);
    }
}
