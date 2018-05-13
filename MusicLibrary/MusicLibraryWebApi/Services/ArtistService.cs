using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryWebApi.Models;
using MusicLibraryWebApi.Services.Interfaces;

namespace MusicLibraryWebApi.Services
{
    [Export(typeof(IArtistService))]
    public class ArtistService : IArtistService
    {
        [Import]
        private IDataService dataService { get; set; }

        [ImportingConstructor]
        public ArtistService()
        { }

        public async Task<IEnumerable<Artist>> GetArtists() => await dataService.GetList<Artist>();

        public async Task<Artist> GetArtist(object id) => await dataService.Get<Artist>(id);

        public async Task<int> InsertArtist(Artist artist) => await dataService.Insert<Artist,int>(artist);

        public async Task<bool> DeleteArtist(int id) => await dataService.Delete<Artist>(id);

        public async Task<bool> DeleteArtist(Artist artist) => await dataService.Delete(artist);

        public async Task<bool> UpdateArtist(Artist artist) => await dataService.Update(artist);
    }
}