using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryMobile.Services.Interfaces
{
    public interface IWebService
    {
        Task<IEnumerable<T>> Get<T>(string url);
    }
}
