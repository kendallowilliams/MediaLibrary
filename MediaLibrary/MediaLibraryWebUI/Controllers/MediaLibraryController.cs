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
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("MediaLibrary", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MediaLibraryController : BaseController
    {
        private readonly MediaLibraryViewModel mediaLibraryViewModel;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public MediaLibraryController(MediaLibraryViewModel mediaLibraryViewModel, IDataService dataService)
        {
            this.mediaLibraryViewModel = mediaLibraryViewModel;
            this.dataService = dataService;
        }

        public async Task<ActionResult> Index()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaLibraryController).Replace(nameof(Controller), string.Empty));

            if (configuration != null)
            {
                mediaLibraryViewModel.Configuration = JsonConvert.DeserializeObject<MediaLibraryConfiguration>(configuration.JsonData) ?? new MediaLibraryConfiguration();
            }

            return View(mediaLibraryViewModel);
        }

        public async Task UpdateConfiguration(MediaLibraryConfiguration mediaLibraryConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaLibraryController).Replace(nameof(Controller), string.Empty));

                if (configuration == null)
                {
                    configuration = new Configuration()
                    {
                        Type = nameof(MediaLibraryController).Replace(nameof(Controller), string.Empty),
                        JsonData = JsonConvert.SerializeObject(mediaLibraryConfiguration)
                    };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(mediaLibraryConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }

        public async Task<ActionResult> MediaLibraryConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaLibraryController).Replace(nameof(Controller), string.Empty));

            if (configuration != null)
            {
                mediaLibraryViewModel.Configuration = JsonConvert.DeserializeObject<MediaLibraryConfiguration>(configuration.JsonData) ?? new MediaLibraryConfiguration();
            }

            return PartialView($"~/Views/Shared/Configurations/{nameof(MediaLibraryConfiguration)}.cshtml", mediaLibraryViewModel.Configuration);
        }
    }
}