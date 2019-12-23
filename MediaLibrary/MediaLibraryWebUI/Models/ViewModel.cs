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
            MusicPlayerId = "music-player";
            VideoPlayerId = "video-player";
            NowPlaying = "Now Playing";
            Configuration = new TConfig();
        }

        public string MusicPlayerId { get; }
        public string VideoPlayerId { get; }
        public TConfig Configuration { get; set; }
        public string NowPlaying { get; set; }
    }
}