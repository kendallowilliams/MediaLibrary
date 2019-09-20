using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Artist: BaseModel
    {
        public Artist(string name)
        {
            Name = name;
        }

        public Artist(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
