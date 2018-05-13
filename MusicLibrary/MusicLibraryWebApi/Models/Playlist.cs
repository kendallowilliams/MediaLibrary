using MusicLibraryWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryWebApi.Models
{
    public class Playlist : BaseModel
    {
        public string Name { get; set; }
    }
}