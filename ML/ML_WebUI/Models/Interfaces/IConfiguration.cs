using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Models.Interfaces
{
    public interface IConfiguration
    {
        int ScrollTop { get; set; }
    }
}