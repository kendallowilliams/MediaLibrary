using MediaLibraryMobile.Views;
using System;
using System.ComponentModel.Composition.Hosting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MediaLibraryDAL.Services.Interfaces;
using System.ComponentModel.Composition;
using System.ComponentModel;
using MediaLibraryMobile.Controllers;

namespace MediaLibraryMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            using (CompositionContainer _container = GetMEF())
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

        public static CompositionContainer GetMEF(params object[] attributedParts)
        {
            CompositionContainer container = default;
            var catalog = new AggregateCatalog();
            var appCatalog = new AssemblyCatalog(typeof(App).Assembly);
            var dalCatalog = new AssemblyCatalog(typeof(IDataService).Assembly);

            catalog.Catalogs.Add(appCatalog);
            catalog.Catalogs.Add(dalCatalog);

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
