using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface IPodcastUIService
    {
        Task<IEnumerable<IGrouping<string, Podcast>>> GetPodcastGroups(PodcastSort sort);
    }
}
