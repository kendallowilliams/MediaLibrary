using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.UIEnums;
using Fody;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Controllers;
using System.IO;
using Newtonsoft.Json;
using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using static MediaLibraryBLL.Enums;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlayerUIService))]
    public class PlayerUIService : BaseUIService, IPlayerUIService
    {
        private readonly Lazy<IDataService> lazyDataService;
        private IDataService dataService => lazyDataService.Value;

        [ImportingConstructor]
        public PlayerUIService(Lazy<IDataService> dataService) : base()
        {
            this.lazyDataService = dataService;
        }
    }
}