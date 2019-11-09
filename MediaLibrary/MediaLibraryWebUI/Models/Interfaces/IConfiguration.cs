using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Interfaces
{
    public interface IConfiguration
    {
        MediaPages SelectedMediaPage { get; set; }
    }
}