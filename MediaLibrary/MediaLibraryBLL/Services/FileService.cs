using Fody;
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
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Models;
using MediaLibraryDAL.Services.Interfaces;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IFileService))]
    public class FileService : IFileService
    {
        private readonly IDataService dataService;
        private readonly IId3Service id3Service;
        private readonly IArtistService artistService;
        private readonly IAlbumService albumService;
        private readonly IGenreService genreService;
        private readonly ITrackService trackService;
        private readonly ITransactionService transactionService;
        private readonly IEnumerable<string> fileTypes;

        [ImportingConstructor]
        public FileService(IId3Service id3Service, IArtistService artistService, IAlbumService albumService,
                           IGenreService genreService, ITrackService trackService, ITransactionService transactionService,
                           IDataService dataService)
        {
            this.id3Service = id3Service;
            this.artistService = artistService;
            this.albumService = albumService;
            this.genreService = genreService;
            this.trackService = trackService;
            this.transactionService = transactionService;
            this.dataService = dataService;
            fileTypes = ConfigurationManager.AppSettings["FileTypes"].Split(new[] { ',' })
                                                                     .Select(fileType => fileType.ToLower());
        }

        public string MusicFolder { get => Path.Combine(RootFolder, "Music"); }

        public string PodcastFolder { get => Path.Combine(RootFolder, "Podcast"); }

        public string RootFolder { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic, Environment.SpecialFolderOption.Create), "MediaLibrary"); }

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

        public void Delete(string path)
        {
            if (File.Exists(path)) { File.Delete(path); }
        }

        public async Task ReadDirectory(Transaction transaction, string path, bool recursive = true)
        {
            try
            {
                IEnumerable<string> allFiles = EnumerateFiles(path, recursive: recursive);
                var fileGroups = allFiles.Where(file => fileTypes.Contains(Path.GetExtension(file).ToLower()))
                                         .GroupBy(file => new { directory = Path.GetDirectoryName(file) });

                foreach (var group in fileGroups)
                {
                    foreach (string file in group) { await ReadMediaFile(file); }
                }

                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }

        public async Task ReadMediaFile(string path)
        {
            MediaData data = await id3Service.ProcessFile(path);
            int? genreId = await genreService.AddGenre(data.Genres),
                artistId = await artistService.AddArtist(data.Artists),
                albumId = await albumService.AddAlbum(new Album(data, artistId, genreId)),
                pathId = await trackService.AddPath(Path.GetDirectoryName(path));
            Track track = new Track(data, pathId, genreId, albumId, artistId);

            await dataService.Insert(track);
        }

        public async Task CheckForMusicUpdates(Transaction transaction)
        {
            try
            {
                IEnumerable<TrackPath> paths = await dataService.GetList<TrackPath>();
                IEnumerable<Album> albumsToDelete = Enumerable.Empty<Album>();
                IEnumerable<Artist> artistsToDelete = Enumerable.Empty<Artist>();

                foreach (TrackPath path in paths)
                {
                    IEnumerable<Track> tracks = await dataService.GetList<Track>(track => track.PathId == path.Id);
                    IEnumerable<string> existingFiles = tracks.Select(track => Path.Combine(path.Location, track.FileName)),
                                        files = EnumerateFiles(path.Location).Where(file => fileTypes.Contains(Path.GetExtension(file).ToLower())),
                                        deletedFiles = existingFiles.Where(file => !File.Exists(file));

                    foreach (string file in files.Except(existingFiles))
                    {
                        await ReadMediaFile(file);
                    }

                    foreach (string file in deletedFiles)
                    {
                        Transaction deleteTransaction = null;

                        try
                        {
                            Track song = tracks.FirstOrDefault(track => track.FileName == Path.GetFileName(file));

                            deleteTransaction = await transactionService.GetNewTransaction(TransactionTypes.RemoveTrack);
                            deleteTransaction.Message = $"Attempting to remove song [ID: {song?.Id}]...";
                            await dataService.Update(deleteTransaction);
                            await dataService.Delete(song);
                            await transactionService.UpdateTransactionCompleted(deleteTransaction, $"Song [ID: {song?.Id}] removed.");
                        }
                        catch(Exception ex)
                        {
                            await transactionService.UpdateTransactionErrored(deleteTransaction, ex);
                        }
                    }

                    albumsToDelete = await dataService.GetList<Album>(album => album.Tracks.Count() == 0, default, album => album.Tracks);
                    artistsToDelete = await dataService.GetList<Artist>(artist => artist.Tracks.Count() == 0, default, artist => artist.Tracks);
                    foreach (Album album in albumsToDelete) { await dataService.Delete<Album>(album.Id); }
                    foreach (Artist artist in artistsToDelete) { await dataService.Delete<Artist>(artist.Id); }
                    path.LastScanDate = DateTime.Now;
                    await dataService.Update(path);
                }

                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }
    }
}