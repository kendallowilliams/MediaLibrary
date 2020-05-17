using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class TelevisionViewModel : ViewModel<TelevisionConfiguration>
    {
        [ImportingConstructor]
        public TelevisionViewModel()
        {
            SeriesGroups = Enumerable.Empty<IGrouping<string, Series>>();
            SeriesSortItems = TelevisionRepository.GetSeriesSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
        }

        public Series SelectedSeries { get; set; }
        public IEnumerable<IGrouping<string, Series>> SeriesGroups { get; set; }
        public IEnumerable<SelectListItem> SeriesSortItems { get; }
        public int MinimumNumberOfSeasons { get => 5; }
        public bool HasPlaylists { get; set; }
    }
}