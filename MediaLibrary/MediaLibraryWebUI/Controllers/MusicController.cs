using MediaLibraryWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryDAL.Models;

namespace MediaLibraryWebUI.Controllers
{
    public class MusicController : Controller
    {
        private readonly IDataService dataService;

        public MusicController(IDataService dataService)
        {
            this.dataService = dataService;
        }
        
        public async Task<ActionResult> Index()
        {
            MusicViewModel model = new MusicViewModel();

            model.Tracks = await dataService.GetList<Track>();

            return View(model);
        }
    }
}