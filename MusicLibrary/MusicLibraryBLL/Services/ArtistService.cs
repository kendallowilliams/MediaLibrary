using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IArtistService))]
    public class ArtistService : IArtistService
    {
        private readonly IDataService dataService;
        private readonly string findArtistsStoredProcedure = "FindArtists",
                                deleteAllArtistsStoredProcedure = "DeleteAllArtists";

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
                IEnumerable<Artist> artists = await dataService.Query<Artist>(findArtistsStoredProcedure, parameters, CommandType.StoredProcedure);

                if (artists.Any()) { id = artists.FirstOrDefault().Id; }
                else { id = await dataService.Insert<Artist, int>(artist); }
            }

            return id;
        }

        public async Task<IEnumerable<Artist>> GetArtists() => await dataService.GetList<Artist>();

        public async Task<Artist> GetArtist(object id) => await dataService.Get<Artist>(id);

        public async Task<int> InsertArtist(Artist artist) => await dataService.Insert<Artist,int>(artist);

        public async Task<bool> DeleteArtist(int id) => await dataService.Delete<Artist>(id);

        public async Task<bool> DeleteArtist(Artist artist) => await dataService.Delete(artist);

        public async Task DeleteAllArtists() => await dataService.Execute(deleteAllArtistsStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdateArtist(Artist artist) => await dataService.Update(artist);
    }
}