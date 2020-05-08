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
        private Action loadMain;

        [ImportingConstructor]
        public LoginController(LoginViewModel loginViewModel, IWebService webService)
        {
            this.webService = webService;
            this.loginViewModel = loginViewModel;
            this.loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
            this.loginViewModel.LoginCommand = new Command(Login);
        }

        public Page GetLoginView() => loginViewModel.View;

        public Action LoadMain { get => loadMain; set => loadMain = value; }

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Login()
        {
            
        }
    }
}
