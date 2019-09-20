﻿using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Playlist: BaseModel
    {
        public Playlist(string name, IEnumerable<int> trackIds = null)
        {
            Name = name;
        }
    }
}
