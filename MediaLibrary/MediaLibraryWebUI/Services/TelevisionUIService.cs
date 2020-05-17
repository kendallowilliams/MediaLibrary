using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.UIEnums;
using Fody;
using MediaLibraryWebUI.Models;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITelevisionUIService))]
    public class TelevisionUIService : BaseUIService, ITelevisionUIService
    {
        private readonly Lazy<IDataService> lazyDataService;
        private IDataService dataService => lazyDataService.Value;

        [ImportingConstructor]
        public TelevisionUIService(Lazy<IDataService> dataService) : base()
        {
            this.lazyDataService = dataService;
        }

        public async Task<IEnumerable<IGrouping<string, Series>>> GetSeriesGroups(SeriesSort sort)
        {
            IEnumerable<IGrouping<string, Series>> groups = null;
            IEnumerable<Series> series = await dataService.GetList<Series>(default, default, s => s.Episodes);

            switch (sort)
            {
                case SeriesSort.AtoZ:
                default:
                    groups = GetSeriessAtoZ(series.OrderBy(s => s.Title));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Series>> GetSeriessAtoZ(IEnumerable<Series> series)
        {
            return series.GroupBy(s => getCharLabel(s.Title)).OrderBy(group => group.Key);
        }
    }
}