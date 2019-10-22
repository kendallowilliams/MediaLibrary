using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.ModelConfigurations
{
    public class MusicConfiguration
    {
        private int selectedAlbumId,
                    selectedArtistId;
        private AlbumSort selectedAlbumSort;
        private ArtistSort selectedArtistSort;
        private SongSort selectedSongSort;
        private MusicTab selectedMusicTab;
        private MusicPages selectedMusicPage;

        public MusicConfiguration()
        {

        }

        public int SelectedAlbumId { get => selectedAlbumId; set => selectedAlbumId = value; }
        public int SelectedArtistId { get => selectedArtistId; set => selectedArtistId = value; }
        public AlbumSort SelectedAlbumSort { get => selectedAlbumSort; set => selectedAlbumSort = value; }
        public ArtistSort SelectedArtistSort { get => selectedArtistSort; set => selectedArtistSort = value; }
        public SongSort SelectedSongSort { get => selectedSongSort; set => selectedSongSort = value; }
        public MusicTab SelectedMusicTab { get => selectedMusicTab; set => selectedMusicTab = value; }
        public MusicPages SelectedMusicPage { get => selectedMusicPage; set => selectedMusicPage = value; }
    }
}