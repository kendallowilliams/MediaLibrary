using Fody;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IFileService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileService : IFileService
    {
        private readonly IId3Service id3Service;
        private readonly IArtistService artistService;
        private readonly IAlbumService albumService;
        private readonly IGenreService genreService;
        private readonly ITrackService trackService;

         [ImportingConstructor]
        public FileService(IId3Service id3Service, IArtistService artistService, IAlbumService albumService,
                           IGenreService genreService, ITrackService trackService)
        {
            this.id3Service = id3Service;
            this.artistService = artistService;
            this.albumService = albumService;
            this.genreService = genreService;
            this.trackService = trackService;
        }

        public async Task<IEnumerable<string>> EnumerateDirectories(string path, string searchPattern = "*", bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return await Task.Run(() => Directory.EnumerateDirectories(path, searchPattern, searchOption));
        }

        public async Task<IEnumerable<string>> EnumerateFiles(string path, string searchPattern = "*", bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return await Task.Run(() => Directory.EnumerateFiles(path, searchPattern, searchOption));
        }

        public async Task Write(string path, byte[] data)
        {
            await Task.Run(() => File.WriteAllBytes(path, data));
        }

        public async Task Write(string path, string data)
        {
            await Task.Run(() => File.WriteAllText(path, data));
        }

        public async Task<bool> Exists(string path) => await Task.Run(() => File.Exists(path));

        public async Task Delete(string path) => await Task.Run(() => File.Delete(path));

        public async Task ReadDirectory(string path, bool recursive = true, bool copyFiles = false)
        {
            IEnumerable<string> fileTypes = ConfigurationManager.AppSettings["FileTypes"].Split(new[] { ',' })
                                                                                         .Select(fileType => fileType.ToLower());
            IEnumerable<string> allFiles = await EnumerateFiles(path, recursive: recursive);
            var fileGroups = allFiles.Where(file => fileTypes.Contains(Path.GetExtension(file).ToLower()))
                                     .GroupBy(file => new { directory = Path.GetDirectoryName(file) });
            foreach(var group in fileGroups)
            {
                foreach (string file in group) { await ReadMediaFile(file, copyFiles); }
            }
        }

        public async Task ReadMediaFile(string path, bool copyFiles = false)
        {
            MediaData data = await id3Service.ProcessFile(path);
            int? genreId = await genreService.AddGenre(data.Genres),
                artistId = await artistService.AddArtist(data.Artists),
                albumId = await albumService.AddAlbum(new Album(data, artistId, genreId)),
                pathId = await trackService.AddPath(Path.GetDirectoryName(path));
            Track track = new Track(data, pathId, genreId, albumId, artistId);
            await trackService.InsertTrack(track);
        }
    }
}