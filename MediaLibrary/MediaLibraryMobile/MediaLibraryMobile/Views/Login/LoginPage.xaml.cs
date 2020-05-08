using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaLibraryMobile.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(ILoginView))]
    public partial class LoginPage : ContentPage, ILoginView
    {
        public LoginPage()
        {
            InitializeComponent();
        }
    }
}