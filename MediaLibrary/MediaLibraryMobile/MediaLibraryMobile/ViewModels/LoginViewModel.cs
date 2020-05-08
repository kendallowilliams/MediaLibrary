using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class LoginViewModel : BaseViewModel<ILoginView>
    {
        [ImportingConstructor]
        public LoginViewModel(ILoginView loginView): base(loginView)
        {

        }
    }
}
