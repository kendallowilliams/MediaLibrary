using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models.Configurations
{
    public abstract class BaseConfiguration : IConfiguration
    {
        public BaseConfiguration() { }

        public int ScrollTop { get; set; }
    }
}