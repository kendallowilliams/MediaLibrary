using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaLibraryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(IMenuView))]
    public partial class MenuPage : ContentPage, IMenuView
    {
        public MenuPage()
        {
            InitializeComponent();
        }
    }
}