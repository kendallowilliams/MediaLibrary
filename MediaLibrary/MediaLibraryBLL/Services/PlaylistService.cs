using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System.Linq.Expressions;
using MediaLibraryDAL.DbContexts;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlaylistService))]
    public class PlaylistService : IPlaylistService
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PlaylistService(IDataService dataService)
        {
            this.dataService = dataService;
        }
    }
}