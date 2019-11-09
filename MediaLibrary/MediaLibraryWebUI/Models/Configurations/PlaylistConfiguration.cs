using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class PlaylistConfiguration : IConfiguration
    {
        public PlaylistConfiguration()
        {
            SelectedMediaPage = MediaPages.Playlists;
        }

        public int SelectedPlaylistId { get; set; }
        public PlaylistPages SelectedPlaylistPage { get; set; }
        public PlaylistSort SelectedPlaylistSort { get; set; }
        public MediaPages SelectedMediaPage { get; set; }
    }
}