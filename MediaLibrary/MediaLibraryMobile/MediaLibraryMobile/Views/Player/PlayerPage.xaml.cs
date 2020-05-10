using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaLibraryMobile.Views.Player
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(IPlayerView))]
    public partial class PlayerPage : ContentPage, IPlayerView
    {
        [ImportingConstructor]
        public PlayerPage()
        {
            InitializeComponent();
        }
    }
}