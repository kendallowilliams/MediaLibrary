using Android.Util;
using MediaLibraryMobile.Services.Interfaces;
using MediaLibraryMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class LoginController
    {
        private readonly IWebService webService;
        private readonly LoginViewModel loginViewModel;
        private readonly ISharedPreferencesService sharedPreferencesService;
        private Uri baseUri;

        [ImportingConstructor]
        public LoginController(LoginViewModel loginViewModel, IWebService webService, ISharedPreferencesService sharedPreferencesService)
        {
            string baseAddress = string.Empty;

            this.webService = webService;
            this.sharedPreferencesService = sharedPreferencesService;
            this.loginViewModel = loginViewModel;
            this.loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
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

        public LoginViewModel GetLoginViewModel() => loginViewModel;

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public async Task Login(Action success = default, Action failure = default)
        {
            if (await webService.IsAuthorized(baseUri, string.Empty, loginViewModel.Username, loginViewModel.Password))
            {
                success?.Invoke();
            }
            else
            {
                failure?.Invoke();
            }
        }
    }
}
