using Fody;
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
    [ConfigureAwait(false)]
    [Export(typeof(IId3Service)), PartCreationPolicy(CreationPolicy.NonShared)]
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
                string fileName = System.IO.Path.GetFileName(file.Name);
                MediaData data = new MediaData
                {
                    Album = tag.Album,
                    Artists = tag.JoinedPerformers,
                    AlbumArtists = tag.JoinedAlbumArtists,
                    Comment = tag.Comment,
                    Copyright = tag.Copyright,
                    FileName = fileName,
                    Duration = (file.Properties.MediaTypes != MediaTypes.None) ? file.Properties.Duration.TotalSeconds : 0,
                    Title = tag.Title ?? fileName,
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