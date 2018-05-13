using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IArtistService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class ArtistService : IArtistService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public ArtistService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int> AddArtist(string name)
        {
            string existsQuery = $"SELECT id FROM artist WHERE name = @name";
            int id = await dataService.ExecuteScalar<int>(existsQuery, new { name });
            Artist artist = new Artist(name);

            if (id == 0)
            {
                id = await dataService.Insert<Artist,int>(artist);
            }

            return id;
        }

        public async Task<IEnumerable<Artist>> GetArtists() => await dataService.GetList<Artist>();

        public async Task<Artist> GetArtist(object id) => await dataService.Get<Artist>(id);

        public async Task<int> InsertArtist(Artist artist) => await dataService.Insert<Artist,int>(artist);

        public async Task<bool> DeleteArtist(int id) => await dataService.Delete<Artist>(id);

        public async Task<bool> DeleteArtist(Artist artist) => await dataService.Delete(artist);

        public async Task<bool> UpdateArtist(Artist artist) => await dataService.Update(artist);
    }
}