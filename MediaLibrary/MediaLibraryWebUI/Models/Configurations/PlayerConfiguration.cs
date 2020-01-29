using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class PlayerConfiguration : BaseConfiguration
    {
        public PlayerConfiguration()
        {
            AutoPlay = true;
            Repeat = RepeatTypes.None;
            SelectedPlayerPage = PlayerPages.Index;
            Volume = 100;
        }

        public MediaTypes SelectedMediaType { get; set; }
        
        public int CurrentItemIndex { get; set; }

        public bool AutoPlay { get; set; }

        public RepeatTypes Repeat { get; set; }

        public bool Shuffle { get; set; }

        public PlayerPages SelectedPlayerPage { get; set; }

        public int Volume { get; set; }
    }
}