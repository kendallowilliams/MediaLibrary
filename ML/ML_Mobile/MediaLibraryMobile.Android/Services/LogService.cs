using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MediaLibraryMobile.Services.Interfaces;
using Android.Util;

namespace MediaLibraryMobile.Droid.Services
{
    [Export(typeof(ILogService))]
    public class LogService : ILogService
    {
        [ImportingConstructor]
        public LogService() { }

        public void Error(string source, string message)
        {
            Log.Error(source, message);
        }

        public void Info(string source, string message)
        {
            Log.Info(source, message);
        }

        public void Warn(string source, string message)
        {
            Log.Warn(source, message);
        }
    }
}