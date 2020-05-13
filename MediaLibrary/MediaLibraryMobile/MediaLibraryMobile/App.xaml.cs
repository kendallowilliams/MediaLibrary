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
    [Export]
    public partial class App : Application
    {
        private readonly MainController mainController;

        [ImportingConstructor]
        public App(MainController mainController)
        {
            InitializeComponent();
            this.mainController = mainController;
        }

        protected override void OnStart()
        {
            MainPage = mainController.GetMainView();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
