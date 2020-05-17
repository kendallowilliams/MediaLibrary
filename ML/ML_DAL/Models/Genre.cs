using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models
{
    public class Genre : BaseModel
    {
        public Genre() { }

        public Genre(string name)
        {
            Name = name;
        }

        public Genre(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }
}