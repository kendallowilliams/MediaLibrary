using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryBLL.Enums;
using static MediaLibraryWebUI.UIEnums;

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
            SkipForwardSeconds = SkipBackwardSeconds = 15;
        }

        public MediaTypes SelectedMediaType { get; set; }

        public int CurrentItemIndex { get; set; }

        public bool AutoPlay { get; set; }

        public RepeatTypes Repeat { get; set; }

        public bool Shuffle { get; set; }

        public PlayerPages SelectedPlayerPage { get; set; }

        public int Volume { get; set; }

        public bool Muted { get; set; }

        public bool AudioVisualizerEnabled { get; set; }

        public int SkipForwardSeconds { get; set; }

        public int SkipBackwardSeconds { get; set; }
    }
}