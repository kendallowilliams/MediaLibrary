using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Genre: IDataModel
    {
        public Genre(string name): base()
        {
            Name = name;
        }

        public Genre(int id, string name): base()
        {
            Id = id;
            Name = name;
        }
    }
}
