using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MediaLibraryMobile.Views.Interfaces;

namespace MediaLibraryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(IPlaylistView))]
    public partial class PlaylistPage : ContentPage, IPlaylistView
    {
        [ImportingConstructor]
        public PlaylistPage()
        {
            InitializeComponent();
        }
    }
}