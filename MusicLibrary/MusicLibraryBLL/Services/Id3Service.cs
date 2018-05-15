using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TagLib;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IId3Service))]
    public class Id3Service : IId3Service
    {

        [ImportingConstructor]
        public Id3Service()
        {
        }

        public async Task<MediaData> ProcessFile(string path)
        {
            MediaData mediaData = default(MediaData);

            try
            {
                File file = await Task.Run(() => File.Create(path));
                Tag tag = file.Tag;
                MediaData data = new MediaData
                {
                    Album = tag.Album,
                    Artists = tag.JoinedPerformers,
                    AlbumArtists = tag.JoinedAlbumArtists,
                    Comment = tag.Comment,
                    Copyright = tag.Copyright,
                    FileName = System.IO.Path.GetFileName(file.Name),
                    Duration = (file.Properties.MediaTypes != MediaTypes.None) ? file.Properties.Duration.TotalSeconds : 0,
                    Title = tag.Title,
                    Track = tag.Track,
                    TrackCount = tag.TrackCount,
                    Year = tag.Year
                };

                mediaData = data;
            }
            catch (Exception ex)
            {
                if (ex is UnsupportedFormatException)
                {

                }
            }

            return mediaData;
        }


    }
}