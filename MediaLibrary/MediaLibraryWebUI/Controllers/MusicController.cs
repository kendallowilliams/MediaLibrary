using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class MusicController : Controller
    {
        private readonly IDataService dataService;

        [ImportingConstructor]
        public MusicController(IDataService dataService)
        {
            this.dataService = dataService;
        }
        
        public async Task<ActionResult> Index()
        {
            MusicViewModel model = new MusicViewModel();

            model.Tracks = await dataService.GetList<Track>();

            return View();
        }
    }
}