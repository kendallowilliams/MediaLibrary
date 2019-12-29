﻿using MediaLibraryBLL.Models;
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
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Television", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class TelevisionController : BaseController
    {
        private readonly ITelevisionUIService televisionService;
        private readonly IDataService dataService;
        private readonly TelevisionViewModel televisionViewModel;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public TelevisionController(ITelevisionUIService televisionService, IDataService dataService, TelevisionViewModel televisionViewModel,
                                    ITransactionService transactionService)
        {
            this.televisionService = televisionService;
            this.dataService = dataService;
            this.televisionViewModel = televisionViewModel;
            this.transactionService = transactionService;
        }

        [CompressContent]
        public async Task<ActionResult> Index()
        {
            ActionResult result = null;
            Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Television));

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
            televisionViewModel.SelectedSeries = await dataService.GetAsync<Series, IEnumerable<Episode>>(item => item.Id == id, item => item.Episodes);

            return PartialView("Series", televisionViewModel);
        }

        public async Task UpdateConfiguration(TelevisionConfiguration televisionConfiguration)
        {
            if (ModelState.IsValid)
            {
                Configuration configuration = await dataService.GetAsync<Configuration>(item => item.Type == nameof(MediaPages.Television));

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

        [AllowAnonymous]
        public async Task<ActionResult> File(int id)
        {
            Episode episode = await dataService.GetAsync<Episode>(item => item.Id == id);
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

            return PartialView("Season", episodes);
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> GetM3UPlaylist(int seriesId, int season)
        {
            IEnumerable<Episode> episodes = await dataService.GetList<Episode>(episode => episode.SeriesId == seriesId && episode.Season == season);
            string domain = Request.Url.GetLeftPart(UriPartial.Authority);
            IEnumerable<string> lines = episodes.Select(episode => $"#EXTINF:0,{episode.Title}{Environment.NewLine}{$"{domain}/Television/File/{episode.Id}"}");

            string data = $"#EXTM3U{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
            byte[] content = Encoding.UTF8.GetBytes(data);

            return new FileContentResult(content, "audio/mpegurl");
        }
    }
}