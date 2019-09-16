using MediaLibraryDAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    public class MusicController : Controller
    {
        private readonly IDataService dataService;

        public MusicController()
        {

        }
        
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}