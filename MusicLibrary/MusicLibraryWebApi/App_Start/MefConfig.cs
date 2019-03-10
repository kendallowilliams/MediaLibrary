using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http;
using MediaLibraryWebApi.Resolvers;
using System.Reflection;
using MediaLibraryBLL.Services.Interfaces;

namespace MediaLibraryWebApi
{
    public static class MefConfig
    {
        public static void Register(HttpConfiguration config)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IDataModel).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IAlbumService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer container = new CompositionContainer(catalog, true);
            config.DependencyResolver = new MefResolver(container);
        }
    }
}