using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
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
    [Export(typeof(ITrackService))]
    public class TrackService : ITrackService
    {
        private readonly IDataService dataService;

         [ImportingConstructor]
        public TrackService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<int?> AddPath(string location)
        {

            int? id = default(int?);

            if (!string.IsNullOrWhiteSpace(location))
            {
                object parameters = new { location };
                TrackPath path = new TrackPath(location),
                          dbPath = await dataService.GetAsync<TrackPath>(item => item.Location.Trim().Equals(location.Trim(), 
                                                                                                             StringComparison.CurrentCultureIgnoreCase));

                if (dbPath != null) { id = dbPath.Id; }
                else
                {
                    await dataService.Insert(path);
                    id = path.Id;
                }
            }

            return id;
        }
    }
}