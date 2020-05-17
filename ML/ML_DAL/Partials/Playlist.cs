using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Playlist: IDataModel
    {
        public Playlist(string name): base()
        {
            Name = name;
        }

        [JsonIgnore]
        public string Description 
        {
            get
            {
                string description = string.Empty;

                switch((PlaylistTypes)this.Type)
                {
                    case PlaylistTypes.Music:
                        description = PlaylistTracks.Count() == 1 ? "1 song" : $"{PlaylistTracks.Count()} songs";
                        break;
                    case PlaylistTypes.Podcast:
                        description = PlaylistPodcastItems.Count() == 1 ? "1 episode" : $"{PlaylistPodcastItems.Count()} episodes";
                        break;
                    case PlaylistTypes.Television:
                        description = PlaylistEpisodes.Count() == 1 ? "1 episode" : $"{PlaylistEpisodes.Count()} episodes";
                        break;
                    default:
                        break;
                }

                return description;
            }
        }
    }
}
