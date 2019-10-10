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
        [ImportingConstructor]
        public PlayerController() { }

        // GET: Player
        public ActionResult Index()
        {
            PlayerViewModel model = new PlayerViewModel();

            return View(model);
        }
    }
}