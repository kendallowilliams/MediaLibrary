using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http;
using MediaLibraryWebUI.Resolvers;
using System.Reflection;
using MediaLibraryBLL.Services.Interfaces;
using System.Web.Mvc;

namespace MediaLibraryWebUI
{
    public static class MefConfig
    {
        public static void Register(HttpConfiguration config)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            MefResolver resolver = null;
            CompositionContainer container = null;

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IDataModel).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IAlbumService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            container = new CompositionContainer(catalog, true);
            resolver = new MefResolver(container);
            DependencyResolver.SetResolver(resolver);
            config.DependencyResolver = resolver;
        }
    }
}