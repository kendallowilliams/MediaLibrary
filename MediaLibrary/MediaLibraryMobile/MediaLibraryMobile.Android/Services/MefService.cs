using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediaLibraryDAL.Services.Interfaces;

namespace MediaLibraryMobile.Droid.Services
{
    public class MefService
    {
        public static CompositionContainer GetMEFContainer(params object[] attributedParts)
        {
            CompositionContainer container;
            var catalog = new AggregateCatalog();
            var baseCatalog = new AssemblyCatalog(typeof(App).Assembly);
            var droidCatalog = new AssemblyCatalog(typeof(MefService).Assembly);
            var dalCatalog = new AssemblyCatalog(typeof(IDataService).Assembly);

            catalog.Catalogs.Add(baseCatalog);
            catalog.Catalogs.Add(droidCatalog);
            catalog.Catalogs.Add(dalCatalog);
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