using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Input;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class LoginViewModel : BaseViewModel<ILoginView>
    {
        private ICommand loginCommand;
        private string username,
                       password;
        private bool rememberMe;

        [ImportingConstructor]
        public LoginViewModel(ILoginView loginView): base(loginView)
        {

        }

        public ICommand LoginCommand { get => loginCommand; set => SetProperty<ICommand>(ref loginCommand, value); }
        public string Username { get => username; set => SetProperty<string>(ref username, value); }
        public string Password { get => password; set => SetProperty<string>(ref password, value); }
        public bool RememberMe { get => rememberMe; set => SetProperty<bool>(ref rememberMe, value); }
    }
}
