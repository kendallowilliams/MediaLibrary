using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MediaLibraryWebUI.Models
{
    public abstract class ViewModel : IViewModel
    {
        public ViewModel()
        {
            Domain = WebConfigurationManager.AppSettings["MediaLibraryDomain"];
        }

        public string MusicPlayerId { get => "music-player"; }

        public string Domain { get; }
    }
}