using Fody;
using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryBLL.Enums;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlayerService))]
    public class PlayerService : IPlayerService
    {
        private readonly string fileNamePrefix;
        private readonly IFileService fileService;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PlayerService(IDataService dataService, IFileService fileService)
        {
            this.dataService = dataService;
            this.fileService = fileService;
            fileNamePrefix = $"{nameof(PlayerService)}_{nameof(PlayerService.UpdateNowPlaying)}";
#if DEV
            fileNamePrefix = $"{fileNamePrefix}_DEV";
#elif DEBUG
            fileNamePrefix = $"{fileNamePrefix}_DEBUG";
#endif
        }

        public async Task UpdatePlayCount(int id, MediaTypes mediaType)
        {
            if (mediaType == MediaTypes.Podcast)
            {
                PodcastItem podcastItem = await dataService.Get<PodcastItem>(item => item.Id == id);

                if (podcastItem != null)
                {
                    podcastItem.PlayCount++;
                    podcastItem.LastPlayedDate = DateTime.Now;
                    await dataService.Update(podcastItem);
                }
            }
            else if (mediaType == MediaTypes.Song)
            {
                Track track = await dataService.Get<Track>(item => item.Id == id);

                if (track != null)
                {
                    track.PlayCount++;
                    track.LastPlayedDate = DateTime.Now;
                    await dataService.Update(track);
                }
            }
            else if (mediaType == MediaTypes.Television)
            {
                Episode episode = await dataService.Get<Episode>(item => item.Id == id);

                if (episode != null)
                {
                    episode.PlayCount++;
                    episode.LastPlayedDate = DateTime.Now;
                    await dataService.Update(episode);
                }
            }
        }

        public async Task UpdatePlayerProgress(int id, MediaTypes mediaType, int progess)
        {
            if (mediaType == MediaTypes.Podcast)
            {
                PodcastItem podcastItem = await dataService.Get<PodcastItem>(item => item.Id == id);

                podcastItem.Progress = progess;
                await dataService.Update(podcastItem);
            }
            else if (mediaType == MediaTypes.Song)
            {
                Track song = await dataService.Get<Track>(item => item.Id == id);

                song.Progress = progess;
                await dataService.Update(song);
            }
            else if (mediaType == MediaTypes.Television)
            {
                Episode episode = await dataService.Get<Episode>(item => item.Id == id);

                episode.Progress = progess;
                await dataService.Update(episode);
            }
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
