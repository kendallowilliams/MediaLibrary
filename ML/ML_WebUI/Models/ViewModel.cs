using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MediaLibraryWebUI.Models
{
    public abstract class ViewModel<TConfig> : IViewModel where TConfig: new()
    {
        public ViewModel()
        {
            Configuration = new TConfig();
        }
        
        public TConfig Configuration { get; set; }
    }
}