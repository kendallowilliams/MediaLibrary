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
    [Export(typeof(IPlaylistsView))]
    public partial class PlaylistsPage : ContentPage, IPlaylistsView
    {
        [ImportingConstructor]
        public PlaylistsPage()
        {
            InitializeComponent();
        }
    }
}