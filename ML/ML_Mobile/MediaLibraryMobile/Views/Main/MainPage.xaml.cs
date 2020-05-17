using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaLibraryMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Export(typeof(IMainView))]
    public partial class MainPage : MasterDetailPage, IMainView
    {
        [ImportingConstructor]
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }
    }
}