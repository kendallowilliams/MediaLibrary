using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using MediaLibraryDAL.Services.Interfaces;

namespace MediaLibraryMobile.Services
{
    public static class MefService
    {
        public static CompositionContainer GetMEFContainer(IEnumerable<Assembly> additionalAssemblies = default, params object[] attributedParts)
        {
            CompositionContainer container;
            var catalog = new AggregateCatalog();
            var baseCatalog = new AssemblyCatalog(typeof(App).Assembly);
            var dalCatalog = new AssemblyCatalog(typeof(IDataService).Assembly);

            catalog.Catalogs.Add(baseCatalog);
            catalog.Catalogs.Add(dalCatalog);
            if (additionalAssemblies != default)
            {
                foreach (var assembly in additionalAssemblies) { catalog.Catalogs.Add(new AssemblyCatalog(assembly)); }
            }
            container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(attributedParts);
            }
            catch (CompositionException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return container;
        }
    }
}