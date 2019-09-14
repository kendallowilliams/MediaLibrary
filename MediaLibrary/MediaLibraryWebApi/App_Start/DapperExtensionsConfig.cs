using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DapperExtensions;
using MediaLibraryBLL.Models.Mappings;

namespace MediaLibraryWebApi
{
    public static class DapperExtensionsConfig
    {
        public static void Initialize()
        {
            DapperAsyncExtensions.SetMappingAssemblies(new[] { typeof(AlbumMapping).Assembly });
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}