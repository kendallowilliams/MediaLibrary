using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Podcast : BaseModel
    {
        public string Title { get; set; }
    }
}