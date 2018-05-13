using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TagLib;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IId3Service))]
    public class Id3Service : IId3Service
    {
        private readonly IFileService fileService;
        private readonly IArtistService artistService;
        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;
        private readonly IGenreService genreService;

        [ImportingConstructor]
        public Id3Service(IFileService fileService, IArtistService artistService, IAlbumService albumService, 
                          ITrackService trackService, IGenreService genreService)
        {
            this.fileService = fileService;
            this.artistService = artistService;
            this.albumService = albumService;
            this.trackService = trackService;
            this.genreService = genreService;
        }

        public async Task<bool> ProcessFile(string path)
        {
            bool isProcessed = false;

            try
            {
                File file = await Task.Run(() => File.Create(path));
                Tag tag = file.Tag;
                int artistId = await artistService.AddArtist(tag.Performers),
                    genreId = await genreService.AddGenre(tag.Genres),
                    albumId = await albumService.AddAlbum(tag.Album, tag.Year, artistId, genreId);
                Track track = new Track();
                
                isProcessed = true;
            }
            catch (Exception ex)
            {
                if (ex is UnsupportedFormatException)
                {

                }

                isProcessed = false;
            }

            return isProcessed;
        }


    }
}