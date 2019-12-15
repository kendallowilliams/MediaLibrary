using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class MediaLibraryViewModel : ViewModel<MediaLibraryConfiguration>
    {
        [ImportingConstructor]
        public MediaLibraryViewModel()
        {
        }
    }
}