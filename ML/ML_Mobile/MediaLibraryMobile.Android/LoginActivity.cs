using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediaLibraryMobile.Controllers;
using MediaLibraryMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Binding;
using XBinding = Xamarin.Forms.Binding;
using XCheckBox = Android.Widget.CheckBox;
using XPlatform = Xamarin.Essentials.Platform;
using Xamarin.Essentials;
using Android.Content.PM;
using MediaLibraryMobile.Services;

namespace MediaLibraryMobile.Droid
{
    [Activity(Label = "Login", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private LoginController loginController;
        private LoginViewModel loginViewModel;
        private Login binding;

        public LoginActivity()
        {
            using var container = MefService.GetMEFContainer();
            loginController = container.GetExportedValue<LoginController>();
            loginViewModel = loginController.LoginViewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState); 
            XPlatform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Login);

            binding = new Login(this);
            binding.btnLogin.Click += LoginClicked;
            binding.txtUsername.TextChanged += (sender, args) => loginViewModel.Username = String.Concat(args.Text);
            binding.txtPassword.TextChanged += (sender, args) => loginViewModel.Password = String.Concat(args.Text);
            binding.chkRememberMe.CheckedChange += (sender, args) => loginViewModel.RememberMe = args.IsChecked;

            if (loginViewModel.RememberMe = Preferences.Get(nameof(LoginViewModel.RememberMe), default(bool), "login"))
            {
                binding.btnLogin.CallOnClick();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            XPlatform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void LoginClicked(object sender, EventArgs args)
        {
            binding.loginLayout.Visibility = ViewStates.Gone;
            binding.progressBarLayout.Visibility = ViewStates.Visible;
            await loginController.Login(LoginSucceeded, LoginFailed);
        }

        private void LoginSucceeded()
        {
            binding.lblError.Text = string.Empty;
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
            Finish();
        }

        private void LoginFailed()
        {
            binding.lblError.Text = "Invalid username/password.";
            binding.loginLayout.Visibility = ViewStates.Visible;
            binding.progressBarLayout.Visibility = ViewStates.Gone;
        }
    }
}