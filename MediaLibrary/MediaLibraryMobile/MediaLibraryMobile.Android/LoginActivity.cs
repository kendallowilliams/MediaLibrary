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
        private readonly LoginController loginController;
        private readonly LoginViewModel loginViewModel;
        private Login binding;

        public LoginActivity()
        {
            using var container = MefService.GetMEFContainer();
            loginController = container.GetExportedValue<LoginController>();
            loginViewModel = loginController.GetLoginViewModel();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            using var _container = MefService.GetMEFContainer();
            ISharedPreferencesService sharedPreferencesService = _container.GetExportedValue<ISharedPreferencesService>();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            binding = new Login(this);
            binding.btnLogin.Click += LoginClicked;
            binding.txtUsername.SetBindingContext(loginViewModel);
            binding.txtUsername.SetBinding(nameof(EditText.Text), new XBinding(nameof(LoginViewModel.Username)));
            binding.txtPassword.SetBindingContext(loginViewModel);
            binding.txtPassword.SetBinding(nameof(EditText.Text), new XBinding(nameof(LoginViewModel.Password)));
            binding.chkRememberMe.SetBindingContext(loginViewModel);
            binding.chkRememberMe.SetBinding(nameof(XCheckBox.Checked), new XBinding(nameof(LoginViewModel.RememberMe)));

            if (bool.TryParse(sharedPreferencesService.GetString(nameof(LoginViewModel.RememberMe)), out bool loggedIn) && loggedIn)
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
            RunOnUiThread(() =>
            {
                StartActivity(typeof(MainActivity));
                Finish();
            });
        }

        private void LoginFailed()
        {
            binding.loginLayout.Visibility = ViewStates.Visible;
            binding.progressBarLayout.Visibility = ViewStates.Gone;
        }
    }
}