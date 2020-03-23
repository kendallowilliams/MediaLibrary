using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface ITelevisionUIService
    {
        Task<IEnumerable<IGrouping<string, Series>>> GetSeriesGroups(SeriesSort sort);
    }
}
