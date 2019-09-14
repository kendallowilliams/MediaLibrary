using Fody;
using MediaLibraryDAL.Models;
using MediaLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IFileService))]
    public class FileService : IFileService
    {
        private readonly IId3Service id3Service;
        private readonly IArtistService artistService;
        private readonly IAlbumService albumService;
        private readonly IGenreService genreService;
        private readonly ITrackService trackService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public FileService(IId3Service id3Service, IArtistService artistService, IAlbumService albumService,
                           IGenreService genreService, ITrackService trackService, ITransactionService transactionService)
        {
            this.id3Service = id3Service;
            this.artistService = artistService;
            this.albumService = albumService;
            this.genreService = genreService;
            this.trackService = trackService;
            this.transactionService = transactionService;
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern = "*", bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", bool recursive = false)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(path, searchPattern, searchOption);
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

        public async Task ReadDirectory(Transaction transaction, string path, bool recursive = true, bool copyFiles = false)
        {
            IEnumerable<string> fileTypes = ConfigurationManager.AppSettings["FileTypes"].Split(new[] { ',' })
                                                                                         .Select(fileType => fileType.ToLower());
            try
            {
                IEnumerable<string> allFiles = EnumerateFiles(path, recursive: recursive);
                var fileGroups = allFiles.Where(file => fileTypes.Contains(Path.GetExtension(file).ToLower()))
                                         .GroupBy(file => new { directory = Path.GetDirectoryName(file) });

                foreach (var group in fileGroups)
                {
                    foreach (string file in group) { await ReadMediaFile(file, copyFiles); }
                }

                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task ReadMediaFile(string path, bool copyFile = false)
        {
            MediaData data = await id3Service.ProcessFile(path);
            int? genreId = await genreService.AddGenre(data.Genres),
                artistId = await artistService.AddArtist(data.Artists),
                albumId = await albumService.AddAlbum(new Album(data, artistId, genreId)),
                pathId = await trackService.AddPath(Path.GetDirectoryName(path));
            Track track = new Track(data, pathId, genreId, albumId, artistId);

            await trackService.InsertTrack(track);
            if (copyFile) { await trackService.AddTrackFile(track.Id); }
        }
    }
}