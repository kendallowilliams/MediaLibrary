using MusicLibraryWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;

namespace MusicLibraryWebApi
{
    public static class MefConfig
    {
        public static CompositionContainer Container;

        public static void Initialize()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IDataModel).Assembly));
            Container = new CompositionContainer(catalog);
        }

        public static void Dispose() => Container.Dispose();
    }
}