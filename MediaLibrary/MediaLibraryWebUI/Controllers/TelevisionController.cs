using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Television", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TelevisionController : BaseController
    {
        private readonly ITelevisionUIService televisionService;
        private readonly IDataService dataService;
        private readonly TelevisionViewModel televisionViewModel;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public TelevisionController(ITelevisionUIService televisionService, IDataService dataService, TelevisionViewModel televisionViewModel,
                                    ITransactionService transactionService)
        {
            this.televisionService = televisionService;
            this.dataService = dataService;
            this.televisionViewModel = televisionViewModel;
            this.transactionService = transactionService;
        }

        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Television));

            if (configuration != null)
            {
                televisionViewModel.Configuration = JsonConvert.DeserializeObject<TelevisionConfiguration>(configuration.JsonData);
            }

            if (televisionViewModel.Configuration.SelectedTelevisionPage == TelevisionPages.Series)
            {
                result = await Get(televisionViewModel.Configuration.SelectedSeriesId);
            }
            else
            {
                televisionViewModel.SeriesGroups = await televisionService.GetSeriesGroups(televisionViewModel.Configuration.SelectedSeriesSort);
                result = PartialView(televisionViewModel);
            }

            return result;
        }

        private async Task<ActionResult> Get(int id)
        {
            televisionViewModel.SelectedSeries = await dataService.GetAsync<Series>(item => item.Id == id);

            return PartialView("Series", televisionViewModel);
        }

        public async Task UpdateConfiguration(TelevisionConfiguration televisionConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Television));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Television), JsonData = JsonConvert.SerializeObject(televisionConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(televisionConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }
    }
}