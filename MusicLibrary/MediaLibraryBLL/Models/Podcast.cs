using MediaLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryBLL.Models
{
    public class Podcast : BaseModel
    {
        public Podcast() { }

        public Podcast(string title, string url, string imageUrl, string description, string author)
        {
            Title = title;
            Url = url;
            ImageUrl = imageUrl;
            Description = description;
            Author = author;
        }
        
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime LastUpdateDate { get; set; } = DateTime.MinValue;
    }
}