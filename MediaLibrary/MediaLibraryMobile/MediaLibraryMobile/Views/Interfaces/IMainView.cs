using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MediaLibraryMobile.Views.Interfaces
{
    public interface IMainView : IView
    {
        Page Master { get; set; }
        Page Detail { get; set; }
        INavigation Navigation { get; }
    }
}
