using System;
using System.Collections.Generic;
using System.Text;

namespace MediaLibraryMobile.Services.Interfaces
{
    public interface ISharedPreferencesService : IDisposable
    {
        void SetString(string key, string value);
        string GetString(string key);
    }
}
