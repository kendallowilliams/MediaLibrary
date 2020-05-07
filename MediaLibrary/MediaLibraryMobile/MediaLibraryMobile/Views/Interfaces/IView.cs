using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MediaLibraryMobile.Views.Interfaces
{
    public interface IView
    {
        object BindingContext { get; set; }
    }
}
