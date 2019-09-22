using Fody;
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

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPodcastService))]
    public class PodcastService : IPodcastService
    {
        private Func<string, string> getLabel;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PodcastService(IDataService dataService)
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

        public async Task<IEnumerable<IGrouping<string, Podcast>>> GetPodcastGroups(PodcastSort sort)
        {
            IEnumerable<IGrouping<string, Podcast>> groups = null;
            IEnumerable<Podcast> podcasts = await dataService.GetList<Podcast>();

            switch(sort)
            {
                case PodcastSort.AtoZ:
                    groups = GetPodcastsAtoZ(podcasts.OrderBy(song => song.Title));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Podcast>> GetPodcastsAtoZ(IEnumerable<Podcast> podcasts)
        {
            return podcasts.GroupBy(podcast => getLabel(podcast.Title)).OrderBy(group => group.Key);
        }
    }
}