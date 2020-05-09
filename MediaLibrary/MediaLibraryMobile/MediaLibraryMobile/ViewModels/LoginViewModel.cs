using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Input;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class LoginViewModel : BaseViewModelNoView
    {
        private string username,
                       password;
        private bool rememberMe;

        [ImportingConstructor]
        public LoginViewModel(): base()
        {

        }

        public string Username { get => username; set => SetProperty<string>(ref username, value); }
        public string Password { get => password; set => SetProperty<string>(ref password, value); }
        public bool RememberMe { get => rememberMe; set => SetProperty<bool>(ref rememberMe, value); }
    }
}
