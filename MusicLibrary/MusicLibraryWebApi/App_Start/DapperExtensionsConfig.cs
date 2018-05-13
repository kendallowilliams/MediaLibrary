using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions;
using MusicLibraryBLL.Models.Mappings;

namespace MusicLibraryWebApi
{
    public static class DapperExtensionsConfig
    {
        public static void Initialize()
        {
            DapperAsyncExtensions.SetMappingAssemblies(new[] { typeof(AlbumMapping).Assembly });
        }
    }
}