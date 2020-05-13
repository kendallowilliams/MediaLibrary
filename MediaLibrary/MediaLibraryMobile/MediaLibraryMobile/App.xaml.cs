using MediaLibraryMobile.Views;
using System;
using System.ComponentModel.Composition.Hosting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MediaLibraryDAL.Services.Interfaces;
using System.ComponentModel.Composition;
using System.ComponentModel;
using MediaLibraryMobile.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace MediaLibraryMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        public IEnumerable<Type> AdditionalTypes { get; set; }

        protected override void OnStart()
        {
            using (CompositionContainer _container = GetMEF(AdditionalTypes))
            {
                MainController controller = _container.GetExportedValue<MainController>();
                MainPage = controller.GetMainView();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static CompositionContainer GetMEF(IEnumerable<Type> additionalTypes = default, params object[] attributedParts)
        {
            CompositionContainer container = default;
            var catalog = new AggregateCatalog();
            var appCatalog = new AssemblyCatalog(typeof(App).Assembly);
            var dalCatalog = new AssemblyCatalog(typeof(IDataService).Assembly);

            catalog.Catalogs.Add(appCatalog);
            catalog.Catalogs.Add(dalCatalog);
            foreach (var type in additionalTypes ?? Enumerable.Empty<Type>()) { catalog.Catalogs.Add(new AssemblyCatalog(type.Assembly)); }

            container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(attributedParts);
            }
            catch(CompositionException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return container;
        }
    }
}
