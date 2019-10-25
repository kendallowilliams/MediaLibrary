using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class PlaylistConfiguration
    {
        private int selectedPlaylistId;
        private PlaylistPages selectedPlaylistPage;
        private PlaylistSort selectedPlaylistSort;

        public PlaylistConfiguration()
        {
        }

        public int SelectedPlaylistId { get => selectedPlaylistId; set => selectedPlaylistId = value; }
        public PlaylistPages SelectedPlaylistPage { get => selectedPlaylistPage; set => selectedPlaylistPage = value; }
        public PlaylistSort SelectedPlaylistSort { get => selectedPlaylistSort; set => selectedPlaylistSort = value; }
    }
}