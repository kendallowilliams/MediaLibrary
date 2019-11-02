using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class MusicConfiguration : IConfiguration
    {
        public MusicConfiguration()
        {
            SelectPage = MediaPages.Music;
        }

        public int SelectedAlbumId { get; set; }
        public int SelectedArtistId { get; set; }
        public AlbumSort SelectedAlbumSort { get; set; }
        public ArtistSort SelectedArtistSort { get; set; }
        public SongSort SelectedSongSort { get; set; }
        public MusicTab SelectedMusicTab { get; set; }
        public MusicPages SelectedMusicPage { get; set; }
        public MediaPages SelectPage { get; set; }
    }
}