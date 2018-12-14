using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using DapperExtensions;
using Fody;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;

namespace MusicLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPodcastService))]
    public class PodcastService : IPodcastService
    {
        private readonly IDataService dataService;
        private readonly IWebService webService;
        private readonly ITransactionService transactionService;
        private readonly string deleteAllPodcastsStoredProcedure = @"DeleteAllPodcasts";

         [ImportingConstructor]
        public PodcastService(IDataService dataService, IWebService webService, ITransactionService transactionService)
        {
            this.dataService = dataService;
            this.webService = webService;
            this.transactionService = transactionService;
        }

        public async Task<Podcast> AddPodcast(string url)
        {
            return await ParseRssFeed(url);
        }

        public async Task<IEnumerable<Podcast>> GetPodcasts() => await dataService.GetList<Podcast>();

        public async Task<Podcast> GetPodcast(object id) =>  await dataService.Get<Podcast>(id);

        public async Task<int> InsertPodcast(Podcast podcast) => await dataService.Insert<Podcast,int>(podcast);

        public async Task<int> InsertPodcastItem(PodcastItem podcastItem) => await dataService.Insert<PodcastItem, int>(podcastItem);

        public async Task<bool> DeletePodcast(int id) => await dataService.Delete<Podcast>(id);

        public async Task<bool> DeletePodcast(Podcast podcast) => await dataService.Delete(podcast);

        public async Task DeleteAllPodcasts() => await dataService.Execute(deleteAllPodcastsStoredProcedure, commandType: CommandType.StoredProcedure);

        public async Task<bool> UpdatePodcast(Podcast podcast) => await dataService.Update(podcast);
        public async Task<bool> UpdatePodcastItem(PodcastItem podcastItem) => await dataService.Update(podcastItem);

        public async Task<Podcast> ParseRssFeed(string address)
        {
            string title = string.Empty,
                   imageUrl = string.Empty,
                   description = string.Empty;
            DateTime pubDate = DateTime.MinValue;
            List<ISyndicationItem> items = new List<ISyndicationItem>();
            IEnumerable<PodcastItem> podcastItems = Enumerable.Empty<PodcastItem>();
            Podcast podcast = null;

            using (var xmlReader = XmlReader.Create(address, new XmlReaderSettings { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);

                while (await feedReader.Read())
                {
                    switch(feedReader.ElementType)
                    {
                        case SyndicationElementType.Category:
                            ISyndicationCategory category = await feedReader.ReadCategory();
                            break;
                        case SyndicationElementType.Content:
                            ISyndicationContent content = await feedReader.ReadContent();
                            if (content.Name == "title") { title = content.Value; }
                            if (content.Name == "pubDate") { DateTime.TryParse(content.Value, out pubDate); }
                            if (content.Name == "description") { description = content.Value; }
                            break;
                        case SyndicationElementType.Image:
                            ISyndicationImage image = await feedReader.ReadImage();
                            imageUrl = image.Url?.AbsoluteUri;
                            break;
                        case SyndicationElementType.Item:
                            ISyndicationItem item = await feedReader.ReadItem();
                            items.Add(item);
                            break;
                        case SyndicationElementType.Link:
                            ISyndicationLink link = await feedReader.ReadLink();
                            break;
                        case SyndicationElementType.Person:
                            ISyndicationPerson person = await feedReader.ReadPerson();
                            break;
                        case SyndicationElementType.None:
                        default:
                            break;
                    }
                }

                podcast = new Podcast(title, address, imageUrl, description) { LastUpdateDate = pubDate == DateTime.MinValue ? DateTime.Now : pubDate };
                podcast.Id = await InsertPodcast(podcast);
                podcastItems = items.Select(item => new
                {
                    item.Title,
                    item.Description,
                    Enclosure = item.Links.FirstOrDefault(linkItem => linkItem.RelationshipType == "enclosure"),
                    PublishDate = item.Published.DateTime

                }).Select(data => new PodcastItem(data.Title, data.Description, data.Enclosure.Uri.OriginalString,
                                                  data.Enclosure.Length, data.PublishDate, podcast.Id));

                foreach (var item in podcastItems) { item.Id = await InsertPodcastItem(item); }
            }

            return podcast;
        }

        public async Task<int?> AddPodcastFile(Transaction transaction, int podcastItemId)
        {
            PodcastFile podcastFile = null;

            try
            {
                byte[] data = null;
                string fileName = string.Empty;
                PodcastItem podcastItem = null;

                podcastItem = await dataService.Get<PodcastItem>(podcastItemId);
                data = await webService.DownloadData(podcastItem.Url);
                fileName = Path.GetFileName((new Uri(podcastItem.Url)).LocalPath);
                podcastFile = new PodcastFile(data, MimeMapping.GetMimeMapping(fileName), podcastItem.PodcastId, podcastItem.Id);
                podcastFile.Id = await dataService.Insert<PodcastFile, int>(podcastFile);
                await transactionService.UpdateTransactionCompleted(transaction);
            }
            catch(Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
            }

            return podcastFile?.Id;
        }

        public async Task<PodcastFile> GetPodcastFile(int id) => await dataService.Get<PodcastFile>(id);
    }
}