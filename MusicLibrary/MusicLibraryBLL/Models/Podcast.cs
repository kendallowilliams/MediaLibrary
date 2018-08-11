using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Podcast : BaseModel
    {
        public Podcast() { }

        public Podcast(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime LastUpdateDate { get; set; } = DateTime.MinValue;
    }
}