using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryMobile.Controllers.Interfaces;
using MediaLibraryMobile.Services.Interfaces;
using MediaLibraryMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MediaLibraryMobile.Controllers
{
    [Export]
    public class LoginController : IController
    {
        private readonly IWebService webService;
        private readonly ILogService logService;
        private readonly LoginViewModel loginViewModel;
        private Uri baseUri;

        [ImportingConstructor]
        public LoginController(LoginViewModel loginViewModel, IWebService webService, ILogService logService)
        {
            this.logService = logService;
            this.webService = webService;
            this.loginViewModel = loginViewModel;
            this.loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
        }

        public LoginViewModel LoginViewModel => loginViewModel;

        public void Startup()
        {
            string baseAddress = string.Empty;
#if DEBUG
            if (string.IsNullOrWhiteSpace(baseAddress = Preferences.Get("BASE_URI_DEBUG", default(string))))
            {
                Preferences.Set("BASE_URI_DEBUG", baseAddress = "http://kserver/MediaLibraryDEV/");
            }
#else
            if (string.IsNullOrWhiteSpace(baseAddress = Preferences.Get("BASE_URI", default(string))))
            {
                Preferences.Set("BASE_URI", baseAddress = "https://media.kowmylk.com/");
            }
#endif
            baseUri = new Uri(baseAddress);

            if (Preferences.Get(nameof(LoginViewModel.RememberMe), default(bool), "login"))
            {
                loginViewModel.Username = Preferences.Get(nameof(LoginViewModel.Username), default(string), "login");
                loginViewModel.Password = Preferences.Get(nameof(LoginViewModel.Password), default(string), "login");
            }
        }

        public void Shutdown()
        {
        }

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public async Task Login(Action success = default, Action failure = default)
        {
            if (await webService.IsAuthorized(baseUri, string.Empty, loginViewModel.Username, loginViewModel.Password))
            {
                Preferences.Set(nameof(LoginViewModel.Username), loginViewModel.Username, "login");
                Preferences.Set(nameof(LoginViewModel.Password), loginViewModel.Password, "login");

                if (loginViewModel.RememberMe)
                {
                    Preferences.Set(nameof(LoginViewModel.RememberMe), true, "login");
                }
                success?.Invoke();
            }
            else
            {
                Preferences.Clear("login");
                failure?.Invoke();
            }
        }
    }
}
