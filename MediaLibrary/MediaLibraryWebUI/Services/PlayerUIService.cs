using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.Enums;
using Fody;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Controllers;
using System.IO;
using Newtonsoft.Json;
using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlayerUIService))]
    public class PlayerUIService : BaseUIService, IPlayerUIService
    {
        private readonly string fileNamePrefix;
        private readonly IDataService dataService;
        private readonly IFileService fileService;

        [ImportingConstructor]
        public PlayerUIService(IDataService dataService, IFileService fileService) : base()
        {
            this.dataService = dataService;
            this.fileService = fileService;
#if DEV
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(PlayerController.UpdateNowPlaying)}_DEV";
#elif DEBUG
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(PlayerController.UpdateNowPlaying)}_DEBUG";
#else
            fileNamePrefix = $"{nameof(PlayerController)}_{nameof(PlayerController.UpdateNowPlaying)}";
#endif
        }

        public async Task<IEnumerable<Track>> GetNowPlayingSongs()
        {
            IEnumerable<int> ids = Enumerable.Empty<int>();
            IEnumerable<Track> songs = Enumerable.Empty<Track>();
            string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json");
            IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();

            if (File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(File.ReadAllText(path));
            ids = items.Select(item => item.Value);
            songs = await dataService.GetList<Track>(item => ids.Contains(item.Id), default, item => item.Album, item => item.Artist);

            return ids.Select(id => songs.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
        }

        public async Task<IEnumerable<PodcastItem>> GetNowPlayingPodcastItems()
        {
            IEnumerable<int> ids = Enumerable.Empty<int>();
            string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json");
            IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
            IEnumerable<PodcastItem> podcastItems = Enumerable.Empty<PodcastItem>();

            if (File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(File.ReadAllText(path));
            ids = items.Select(item => item.Value);
            podcastItems = await dataService.GetList<PodcastItem>(item => ids.Contains(item.Id), default, item => item.Podcast);

            return ids.Select(id => podcastItems.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
        }

        public async Task<IEnumerable<Episode>> GetNowPlayingEpisodes()
        {
            IEnumerable<int> ids = Enumerable.Empty<int>();
            string path = Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json");
            IEnumerable<ListItem<int, int>> items = Enumerable.Empty<ListItem<int, int>>();
            IEnumerable<Episode> episodes = Enumerable.Empty<Episode>();

            if (File.Exists(path)) /*then*/ items = JsonConvert.DeserializeObject<IEnumerable<ListItem<int, int>>>(File.ReadAllText(path));
            ids = items.Select(item => item.Value);
            episodes = await dataService.GetList<Episode>(item => ids.Contains(item.Id), default, item => item.Series);

            return ids.Select(id => episodes.FirstOrDefault(item => item.Id == id)).Where(item => item != null);
        }

        public void UpdateNowPlaying(IEnumerable<ListItem<int, int>> items, MediaTypes mediaType)
        {
            if (items != null)
            {
                string data = JsonConvert.SerializeObject(items);

                if (!Directory.Exists(fileService.RootFolder)) /*then*/ Directory.CreateDirectory(fileService.RootFolder);

                if (mediaType == MediaTypes.Song)
                {
                    File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json"), data);
                }
                else if (mediaType == MediaTypes.Podcast)
                {
                    File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json"), data);
                }
                else if (mediaType == MediaTypes.Television)
                {
                    File.WriteAllText(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json"), data);
                }
            }
        }

        public void ClearNowPlaying(MediaTypes mediaType)
        {
            if (mediaType == MediaTypes.Song)
            {
                File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Song)}.json"));
            }
            else if (mediaType == MediaTypes.Podcast)
            {
                File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Podcast)}.json"));
            }
            else if (mediaType == MediaTypes.Television)
            {
                File.Delete(Path.Combine(fileService.RootFolder, $"{fileNamePrefix}_{nameof(MediaTypes.Television)}.json"));
            }
        }
    }
}