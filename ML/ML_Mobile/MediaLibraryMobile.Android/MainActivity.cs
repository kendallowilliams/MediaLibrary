using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MediaLibraryMobile.Services;
using MediaLibraryMobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MediaLibraryMobile.Droid
{
    [Activity(Label = "MediaLibraryMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly CompositionContainer container;
        private App app;

        [Import]
        private ILogService logService;

        public MainActivity()
        {
            container = MefService.GetMEFContainer(new List<Assembly>() { typeof(MainActivity).Assembly }, this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Lazy<App> lazyApp = container.GetExport<App>();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LoadApplication(app = lazyApp.Value);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            container?.Dispose();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            container?.Dispose();
        }
    }
}