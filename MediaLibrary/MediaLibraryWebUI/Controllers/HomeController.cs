using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Controllers
{
    [Export(nameof(MediaPages.Home), typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : BaseController
    {
        private readonly HomeViewModel homeViewModel;
        private readonly Lazy<ITransactionService> lazyTransactionService;
        private readonly Lazy<IDataService> lazyDataService;
        private ITransactionService transactionService => lazyTransactionService.Value;
        private IDataService dataService => lazyDataService.Value;

        [ImportingConstructor]
        public HomeController(HomeViewModel homeViewModel, Lazy<ITransactionService> transactionService, Lazy<IDataService> dataService)
        {
            this.homeViewModel = homeViewModel;
            this.lazyTransactionService = transactionService;
            this.lazyDataService = dataService;
        }

        public ActionResult Index()
        {
            return PartialView(homeViewModel);
        }

        public async Task<ActionResult> HomeConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Home));

            if (configuration != null)
            {
                homeViewModel.Configuration = JsonConvert.DeserializeObject<HomeConfiguration>(configuration.JsonData) ?? new HomeConfiguration();
            }

            return Json(homeViewModel.Configuration, JsonRequestBehavior.AllowGet);
        }
    }
}