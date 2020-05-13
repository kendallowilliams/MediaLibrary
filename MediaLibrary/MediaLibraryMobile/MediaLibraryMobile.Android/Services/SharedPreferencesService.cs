using Android.Content;
using Android.Preferences;
using MediaLibraryMobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace MMediaLibraryMobile.Droid.Services
{
    [Export(typeof(ISharedPreferencesService))]
    public class SharedPreferencesService : ISharedPreferencesService, IDisposable
    {
        private readonly ISharedPreferences sharedPreferences;
        private readonly ISharedPreferencesEditor sharedPreferencesEditor;
        private readonly Context context;

        [ImportingConstructor]
        public SharedPreferencesService()
        {
            this.context = Android.App.Application.Context;
            sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
            sharedPreferencesEditor = sharedPreferences.Edit();
        }

        public void SetString(string key, string value)
        {
            sharedPreferencesEditor.PutString(key, value);
            sharedPreferencesEditor.Commit();
        }

        public string GetString(string key)
        {
            return sharedPreferences.GetString(key, default);
        }

        public void Dispose()
        {
            if (sharedPreferences != null) { sharedPreferences.Dispose(); }
            if (sharedPreferencesEditor != null) { sharedPreferencesEditor.Dispose(); }
        }
    }
}
