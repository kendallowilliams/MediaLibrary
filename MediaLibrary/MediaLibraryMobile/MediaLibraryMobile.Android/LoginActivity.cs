using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using MediaLibraryMobile.Droid.Services;
using MediaLibraryMobile.Services.Interfaces;
using XBinding = Xamarin.Forms.Binding;
using XCheckBox = Android.Widget.CheckBox;

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
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            using var container = MefService.GetMEFContainer();
            loginController = container.GetExportedValue<LoginController>();
            loginViewModel = loginController.LoginViewModel;

            binding = new Login(this);
            binding.btnLogin.Click += LoginClicked;
            binding.txtUsername.TextChanged += (sender, args) => loginViewModel.Username = String.Concat(args.Text);
            binding.txtPassword.TextChanged += (sender, args) => loginViewModel.Password = String.Concat(args.Text);
            binding.chkRememberMe.CheckedChange += (sender, args) => loginViewModel.RememberMe = args.IsChecked;

            if (bool.TryParse(loginController.SharedPreferencesService.GetString(nameof(LoginViewModel.RememberMe)), out bool loggedIn) && loggedIn)
            {
                binding.btnLogin.CallOnClick();
            }
        }

        private async void LoginClicked(object sender, EventArgs args)
        {
            binding.loginLayout.Visibility = ViewStates.Gone;
            binding.progressBarLayout.Visibility = ViewStates.Visible;
            await loginController.Login(LoginSucceeded, LoginFailed);
        }

        private void LoginSucceeded()
        {
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
        }

        private void LoginFailed()
        {
            binding.loginLayout.Visibility = ViewStates.Visible;
            binding.progressBarLayout.Visibility = ViewStates.Gone;
        }
    }
}