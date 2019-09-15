using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaLibraryDAL.Models;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    public class MusicController : Controller
    {
        private readonly IDataService dataService;

        public MusicController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<IActionResult> Index()
        {
            MusicViewModel model = new MusicViewModel();

            model.Tracks = await dataService.GetList<Track>();
            return View();
        }
    }
}