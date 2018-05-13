using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Genre : BaseModel
    {
        public Genre() { }

        public Genre(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}