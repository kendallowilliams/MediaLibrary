using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
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

        [ImportingConstructor]
        public PodcastService(IDataService dataService, IWebService webService)
        {
            this.dataService = dataService;
            this.webService = webService;
        }

        public async Task AddPodcast(Podcast podcast)
        {
            byte[] data = await webService.DownloadData(podcast.Url);
            await ParseRssFeed(podcast.Url);
        }

        public async Task<IEnumerable<Podcast>> GetPodcasts() => await dataService.GetList<Podcast>();

        public async Task<Podcast> GetPodcast(object id) =>  await dataService.Get<Podcast>(id);

        public async Task<int> InsertPodcast(Podcast podcast) => await dataService.Insert<Podcast,int>(podcast);

        public async Task<bool> DeletePodcast(int id) => await dataService.Delete<Podcast>(id);

        public async Task<bool> DeletePodcast(Podcast podcast) => await dataService.Delete(podcast);

        public async Task DeleteAllPodcasts() => await dataService.Execute(@"DELETE podcast;");

        public async Task<bool> UpdatePodcast(Podcast podcast) => await dataService.Update(podcast);

        public async Task<Podcast> ParseRssFeed(string address)
        {
            Podcast podcast = new Podcast();

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
                            break;
                        case SyndicationElementType.Image:
                            ISyndicationImage image = await feedReader.ReadImage();
                            break;
                        case SyndicationElementType.Item:
                            ISyndicationItem item = await feedReader.ReadItem();
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
            }

            return podcast;
        }
    }
}