using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.Models;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;
using MediaLibraryBLL.Services.Interfaces;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IArtistService))]
    public class ArtistService : IArtistService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public ArtistService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int?> AddArtist(string strArtists)
        {
            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(strArtists))
            {
                object parameters = new { name = strArtists };
                Artist artist = new Artist(strArtists);
                Artist dbArtist = await dataService.Get<Artist>(item => item.Name == strArtists);

                if (dbArtist != null) { id = dbArtist.Id; }
                else
                {
                    await dataService.Insert(artist);
                    id = artist.Id;
                }
            }

            return id;
        }

        public async Task<IEnumerable<Artist>> GetArtists(Expression<Func<Artist, bool>> expression = null) => await dataService.GetList(expression);

        public async Task<Artist> GetArtist(Expression<Func<Artist, bool>> expression = null) => await dataService.Get(expression);

        public async Task<int> InsertArtist(Artist artist) => await dataService.Insert<Artist>(artist);

        public async Task<int> DeleteArtist(int id) => await dataService.Delete<Artist>(id);

        public async Task<int> DeleteArtist(Artist artist) => await dataService.Delete(artist);

        public async Task DeleteAllArtists() => await dataService.DeleteAll<Artist>();

        public async Task<int> UpdateArtist(Artist artist) => await dataService.Update(artist);
    }
}