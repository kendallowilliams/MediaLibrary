using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MediaLibraryMobile.Services;
using System;
using System.ComponentModel.Composition.Hosting;

namespace MediaLibraryMobile.Droid
{
    [Activity(Label = "MediaLibraryMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly CompositionContainer container;
        private App app;

        public MainActivity()
        {
            container = MefService.GetMEFContainer();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Lazy<App> lazyApp = container.GetExport<App>();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(app = lazyApp.Value);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            app.Dispose();
            base.OnDestroy();
        }
    }
}