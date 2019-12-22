using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.Enums;
using Fody;
using MediaLibraryWebUI.Models;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(ITelevisionUIService))]
    public class TelevisionUIService : ITelevisionUIService
    {
        private Func<string, string> getLabel;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public TelevisionUIService(IDataService dataService)
        {
            this.dataService = dataService;
            getLabel = title =>
            {
                char first = title.ToUpper().First();
                string label = string.Empty;

                if (Char.IsLetter(first)) { label = first.ToString(); }
                else if (Char.IsDigit(first)) { label = "#"; }
                else label = "&";

                return label;
            };
        }

        public async Task<IEnumerable<IGrouping<string, Series>>> GetSeriesGroups(SeriesSort sort)
        {
            IEnumerable<IGrouping<string, Series>> groups = null;
            IEnumerable<Series> series = await dataService.GetList<Series, IEnumerable<Episode>>(includeExpression: s => s.Episodes);

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
            return series.GroupBy(s => getLabel(s.Title)).OrderBy(group => group.Key);
        }
    }
}