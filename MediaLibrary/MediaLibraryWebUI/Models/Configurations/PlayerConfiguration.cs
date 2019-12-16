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
            AutoPlay = true;
            Repeat = RepeatTypes.None;
        }

        public MediaTypes SelectedMediaType { get; set; }
        
        public int CurrentItemIndex { get; set; }

        public bool AutoPlay { get; set; }

        public RepeatTypes Repeat { get; set; }
    }
}