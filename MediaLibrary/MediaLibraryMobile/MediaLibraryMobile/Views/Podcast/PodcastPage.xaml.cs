using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLibraryMobile.Views.Interfaces;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaLibraryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(IPodcastView))]
    public partial class PodcastPage : ContentPage, IPodcastView
    {
        [ImportingConstructor]
        public PodcastPage()
        {
            InitializeComponent();
        }
    }
}