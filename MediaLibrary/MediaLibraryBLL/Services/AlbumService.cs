using System;
using System.Collections.Generic;
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
    [Export(typeof(IAlbumService))]
    public class AlbumService : IAlbumService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public AlbumService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int?> AddAlbum(Album album)
        {
            int? id = default(int?);

            if (album != null)
            {
                Album dbAlbum = await dataService.GetAsync<Album>(item => item.Title == album.Title);

                if (dbAlbum != null) { id = dbAlbum.Id; }
                else
                {
                    await dataService.Insert(album);
                    id = album.Id;
                }
            }

            return id;
        }
    }
}