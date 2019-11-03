using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MediaLibraryWebUI.Models
{
    public abstract class ViewModel<TConfig> : IViewModel
    {
        public ViewModel()
        {
            MusicPlayerId = "music-player";
        }

        public string MusicPlayerId { get; }
        public TConfig Configuration { get; set; }
    }
}