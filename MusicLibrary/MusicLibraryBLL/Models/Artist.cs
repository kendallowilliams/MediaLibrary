using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Artist : BaseModel
    {
        public Artist() { }

        public Artist(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}