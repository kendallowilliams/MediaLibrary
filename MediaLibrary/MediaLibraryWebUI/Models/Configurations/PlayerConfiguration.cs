using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class PlayerConfiguration : IConfiguration
    {
        public PlayerConfiguration()
        {
            SelectedMediaPage = MediaPages.Player;
        }

        public MediaPages SelectedMediaPage { get; set; }
    }
}