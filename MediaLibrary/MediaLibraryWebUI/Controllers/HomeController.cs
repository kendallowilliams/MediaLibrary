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
    public class HomeController : Controller
    {
        [ImportingConstructor]
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            return View(model);
        }
    }
}