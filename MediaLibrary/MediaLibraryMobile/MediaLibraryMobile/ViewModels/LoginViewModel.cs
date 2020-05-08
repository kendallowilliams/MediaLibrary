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

        [ImportingConstructor]
        public LoginViewModel(ILoginView loginView): base(loginView)
        {

        }

        public ICommand LoginCommand { get => loginCommand; set => SetProperty<ICommand>(ref loginCommand, value); }
    }
}
