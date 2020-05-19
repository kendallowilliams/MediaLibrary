using MediaLibraryMobile.Views;
using System;
using System.ComponentModel.Composition.Hosting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel.Composition;
using System.ComponentModel;
using MediaLibraryMobile.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace MediaLibraryMobile
{
    [Export]
    public partial class App : Application, IDisposable
    {
        private readonly Lazy<MainController> lazyMainController;

        [ImportingConstructor]
        public App(Lazy<MainController> lazyMainController)
        {
            InitializeComponent();
            this.lazyMainController = lazyMainController;
            lazyMainController.Value.Startup();
            Device.SetFlags(new string[] { "MediaElement_Experimental" });
            MainPage = lazyMainController.Value.GetMainView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void Dispose()
        {
            lazyMainController.Value.Shutdown();
        }
    }
}
