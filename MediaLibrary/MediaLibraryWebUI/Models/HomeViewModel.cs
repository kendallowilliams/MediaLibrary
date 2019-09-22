using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class HomeViewModel
    {
        private string musicPlayerId = "media-player";

        [ImportingConstructor]
        public HomeViewModel() { }

        public string MusicPlayerId { get => musicPlayerId; }
    }
}