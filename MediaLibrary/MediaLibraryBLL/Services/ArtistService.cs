using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;

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
                Artist dbArtist = await dataService.GetAsync<Artist>(item => item.Name == strArtists);

                if (dbArtist != null) { id = dbArtist.Id; }
                else
                {
                    await dataService.Insert(artist);
                    id = artist.Id;
                }
            }

            return id;
        }
    }
}