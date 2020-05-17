using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.ActionResults;
using MediaLibraryWebUI.Attributes;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryDAL.Enums;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Controllers
{
    [Export(nameof(MediaPages.Television), typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TelevisionController : BaseController
    {
        private readonly Lazy<ITelevisionUIService> lazyTelevisionService;
        private readonly Lazy<IDataService> lazyDataService;
        private readonly Lazy<TelevisionViewModel> lazyTelevisionViewModel;
        private readonly Lazy<ITransactionService> lazyTransactionService;
        private ITelevisionUIService televisionService => lazyTelevisionService.Value;
        private IDataService dataService => lazyDataService.Value;
        private TelevisionViewModel televisionViewModel => lazyTelevisionViewModel.Value;
        private ITransactionService transactionService => lazyTransactionService.Value;

        [ImportingConstructor]
        public TelevisionController(Lazy<ITelevisionUIService> televisionService, Lazy<IDataService> dataService, Lazy<TelevisionViewModel> televisionViewModel,
                                    Lazy<ITransactionService> transactionService)
        {
            this.lazyTelevisionService = televisionService;
            this.lazyDataService = dataService;
            this.lazyTelevisionViewModel = televisionViewModel;
            this.lazyTransactionService = transactionService;
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Television));

            if (configuration != null)
            {
                televisionViewModel.Configuration = JsonConvert.DeserializeObject<TelevisionConfiguration>(configuration.JsonData);
            }

            if (televisionViewModel.Configuration.SelectedTelevisionPage == TelevisionPages.Series &&
                await dataService.Exists<Series>(item => item.Id == televisionViewModel.Configuration.SelectedSeriesId))
            {
                result = await Get(televisionViewModel.Configuration.SelectedSeriesId);
            }
            else
            {
                televisionViewModel.SeriesGroups = await televisionService.GetSeriesGroups(televisionViewModel.Configuration.SelectedSeriesSort);
                result = PartialView(televisionViewModel);
            }

            return result;
        }

        private async Task<ActionResult> Get(int id)
        {
            televisionViewModel.SelectedSeries = await dataService.Get<Series>(item => item.Id == id, default, item => item.Episodes);

            return PartialView("Series", televisionViewModel);
        }

        public async Task UpdateConfiguration(TelevisionConfiguration televisionConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Television));

                if (configuration == null)
                {
                    configuration = new Configuration() { Type = nameof(MediaPages.Television), JsonData = JsonConvert.SerializeObject(televisionConfiguration) };
                    await dataService.Insert(configuration);
                }
                else
                {
                    configuration.JsonData = JsonConvert.SerializeObject(televisionConfiguration);
                    await dataService.Update(configuration);
                }
            }
        }

#if !DEBUG && !DEV
        [AllowAnonymous]
#endif
        public async Task<ActionResult> File(int id)
        {
            Episode episode = await dataService.Get<Episode>(item => item.Id == id);
            ActionResult result = null;

            if (episode != null)
            {
                result = new FileRangeResult(episode.Path,
                                             Request.Headers["Range"],
                                             MimeMapping.GetMimeMapping(Path.GetFileName(episode.Path)));
            }
            else
            {
                result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task<ActionResult> GetSeason(int series, int season)
        {
            IEnumerable<Episode> episodes = await dataService.GetList<Episode>(item => item.SeriesId == series && item.Season == season);
            bool hasPlaylists = await dataService.Exists<Playlist>(item => item.Type == (int)PlaylistTabs.Television);

            return PartialView("Season", (hasPlaylists, episodes));
        }

#if !DEBUG && !DEV
        [AllowAnonymous]
#endif
        public async Task<ActionResult> GetM3UPlaylist(int seriesId, int season)
        {
            IEnumerable<Episode> episodes = await dataService.GetList<Episode>(episode => episode.SeriesId == seriesId && episode.Season == season);
            string path = $"{Request.Url.GetLeftPart(UriPartial.Authority)}{Request.ApplicationPath}";
            IEnumerable<string> lines = episodes.Select(episode => $"#EXTINF:0,{episode.Title}{Environment.NewLine}{$"{path}/Television/File/{episode.Id}"}");
            Series series = await dataService.Get<Series>(item => item.Id == seriesId);
            string data = $"#EXTM3U{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
            byte[] content = Encoding.UTF8.GetBytes(data);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(content, "audio/mpegurl", $"{series.Title.Trim()}_S{season}_{timestamp}");
        }
        
        public async Task<ActionResult> TelevisionConfiguration()
        {
            Configuration configuration = await dataService.Get<Configuration>(item => item.Type == nameof(MediaPages.Television));

            if (configuration != null)
            {
                televisionViewModel.Configuration = JsonConvert.DeserializeObject<TelevisionConfiguration>(configuration.JsonData) ?? new TelevisionConfiguration();
            }

            return Json(televisionViewModel.Configuration, JsonRequestBehavior.AllowGet);
        }

        public async Task AddEpisodeToPlaylist(int itemId, int playlistId)
        {
            PlaylistEpisode item = new PlaylistEpisode() { PlaylistId = playlistId, EpisodeId = itemId };
            Transaction transaction = null;

            try
            {
                transaction = await transactionService.GetNewTransaction(TransactionTypes.AddPlaylistEpisode);
                await dataService.Insert(item);
                await transactionService.UpdateTransactionCompleted(transaction, $"Playlist: {playlistId}, Track: {itemId}");
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }
        }
    }
}