using Android.Util;
using MediaLibraryMobile.Services.Interfaces;
using MediaLibraryMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using Xamarin.Forms;

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class LoginController
    {
        private readonly IWebService webService;
        private readonly LoginViewModel loginViewModel;
        private readonly ISharedPreferencesService sharedPreferencesService;
        private Action loadMain;
        private Uri baseUri;

        [ImportingConstructor]
        public LoginController(LoginViewModel loginViewModel, IWebService webService, ISharedPreferencesService sharedPreferencesService)
        {
            string baseAddress = string.Empty;

            this.webService = webService;
            this.sharedPreferencesService = sharedPreferencesService;
            this.loginViewModel = loginViewModel;
            this.loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
            this.loginViewModel.LoginCommand = new Command(Login);
#if DEBUG
            if (string.IsNullOrWhiteSpace(baseAddress = this.sharedPreferencesService.GetString("BASE_URI_DEBUG")))
            {
                this.sharedPreferencesService.SetString("BASE_URI_DEBUG", baseAddress = "http://kserver/MediaLibraryDEV/");
            }
#else
            if (string.IsNullOrWhiteSpace(baseAddress = this.sharedPreferencesService.GetString("BASE_URI")))
            {
                this.sharedPreferencesService.SetString("BASE_URI", baseAddress = "https://kserver/MediaLibrary/");
            }
#endif
            baseUri = new Uri(baseAddress);
        }

        public Page GetLoginView() => loginViewModel.View;

        public Action LoadMain { get => loadMain; set => loadMain = value; }

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private async void Login()
        {
            if (await webService.IsAuthorized(baseUri, string.Empty, loginViewModel.Username, loginViewModel.Password))
            {
                loadMain();
            }
        }
    }
}
