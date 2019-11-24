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
    public partial class Playlist: IDataModel
    {
        public Playlist(string name)
        {
            Name = name;
            PlaylistTracks = new HashSet<PlaylistTrack>();
        }
    }
}
