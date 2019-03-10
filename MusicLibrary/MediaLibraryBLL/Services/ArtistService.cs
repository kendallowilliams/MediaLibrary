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
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;

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
                Artist dbArtist = dataService.Get<Artist>(item => item.Name == strArtists);

                if (dbArtist != null) { id = dbArtist.Id; }
                else { id = await dataService.Insert(artist); }
            }

            return id;
        }

        public IEnumerable<Artist> GetArtists(Expression<Func<Artist, bool>> expression = null) => dataService.GetList(expression);

        public Artist GetArtist(Expression<Func<Artist, bool>> expression = null) => dataService.Get(expression);

        public async Task<int> InsertArtist(Artist artist) => await dataService.Insert<Artist>(artist);

        public async Task<int> DeleteArtist(int id) => await dataService.Delete<Artist>(id);

        public async Task<int> DeleteArtist(Artist artist) => await dataService.Delete(artist);

        public async Task DeleteAllArtists() => await dataService.DeleteAll<Artist>();

        public async Task<int> UpdateArtist(Artist artist) => await dataService.Update(artist);
    }
}