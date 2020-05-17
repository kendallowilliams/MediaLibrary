using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models.Interfaces
{
    public interface IPlayableItem : IDataModel
    {
        int PlayCount { get; }
        DateTime? LastPlayedDate { get; }
    }
}