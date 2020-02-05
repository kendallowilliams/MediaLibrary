using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PlayerViewModel : ViewModel<PlayerConfiguration>
    {
        [ImportingConstructor]
        public PlayerViewModel() { }

        public IEnumerable<Track> Songs { get; set; }

        public IEnumerable<PodcastItem> PodcastItems { get; set; }

        public IEnumerable<Episode> Episodes { get; set; }

        public int NumberOfSecondsBeforeRestart { get => 5; }

        public Color CanvasColor { get => Color.FromArgb(200, 200, 200); }
    }
}