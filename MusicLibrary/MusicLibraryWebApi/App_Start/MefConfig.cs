using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http;
using MusicLibraryWebApi.Resolvers;
using System.Reflection;

namespace MusicLibraryWebApi
{
    public static class MefConfig
    {
        public static void Register(HttpConfiguration config)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IDataModel).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer container = new CompositionContainer(catalog);
            config.DependencyResolver = new MefResolver(container);
        }
    }
}