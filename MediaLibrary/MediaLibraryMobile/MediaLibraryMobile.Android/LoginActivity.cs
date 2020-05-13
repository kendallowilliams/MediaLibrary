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
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            binding = new Login(this);
            binding.btnLogin.Click += LoginClicked;
            binding.txtUsername.SetBindingContext(loginViewModel);
            binding.txtUsername.SetBinding(nameof(LoginViewModel.Username), new Xamarin.Forms.Binding(nameof(EditText.Text)));
            binding.txtPassword.SetBindingContext(loginViewModel);
            binding.txtPassword.SetBinding(nameof(LoginViewModel.Password), new Xamarin.Forms.Binding(nameof(EditText.Text)));
            binding.chkRememberMe.SetBindingContext(loginViewModel);
            binding.chkRememberMe.SetBinding(nameof(LoginViewModel.RememberMe), new Xamarin.Forms.Binding(nameof(Android.Widget.CheckBox.Checked)));
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