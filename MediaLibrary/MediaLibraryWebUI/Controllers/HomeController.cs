using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Home", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : BaseController
    {
        private readonly HomeViewModel homeViewModel;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public HomeController(HomeViewModel homeViewModel, ITransactionService transactionService)
        {
            this.homeViewModel = homeViewModel;
            this.transactionService = transactionService;
        }

        public ActionResult Index()
        {
            return View(homeViewModel);
        }
    }
}