using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public abstract class ViewModel : IViewModel
    {
        public string MusicPlayerId => "music-player";
    }
}