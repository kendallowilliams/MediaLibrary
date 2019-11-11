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
    [Export("Player", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlayerController : BaseController
    {
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public PlayerController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }
        
        public ActionResult Index()
        {
            PlayerViewModel model = new PlayerViewModel();

            return View(model);
        }
    }
}